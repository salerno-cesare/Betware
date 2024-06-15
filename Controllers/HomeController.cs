using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Betware.Models.MyBetViewModel;
using Betware.Data;
using Microsoft.EntityFrameworkCore;
using Betware.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using ECMS.Helpers;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Betware.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext DbContext;
        private readonly ManageController _manager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;

        public HomeController(ApplicationDbContext context, ILoggerFactory loggerFactory, UserManager<ApplicationUser> userManager)
        {

            DbContext = context;
            _logger = loggerFactory.CreateLogger<HomeController>();
            _userManager = userManager;

        }
        [Authorize]
        public IActionResult Index()
        {
            try
            {
                var user = User.Identity.Name;
                ApplicationUser userLogging = _userManager.Users.Where(x => x.Email.Equals(user)).FirstOrDefault();
                Standings classifca = DbContext.Standings.Where(x => x.User.Email.Equals(user)).FirstOrDefault();
                ViewBag.Position = classifca.Position;
                ViewBag.NameLogIn = userLogging.FirstName;
            }
            catch (Exception e)
            {
                _logger.LogError("--> Error in Index" + User + " Ex: " + e.Message);
                return View("Error");
            }
            return View();
        }
        [Authorize]
        //[Route ("Home/MyBet/{email}")]
        public async Task<IActionResult> MyBet(string email)
        {
            try
            {

                ViewBag.saveBet = false;
                ViewBag.canread = false;
                if (User == null)
                    return RedirectToAction("Index", "Error", new { id = 403 });
                var userEmail = User.Identity.Name;
                IList<Bet> betUser = new List<Bet>();
                IList<FinalSession> newMyBetSesison = new List<FinalSession>();
                if (email != null && !userEmail.Equals(email))
                {
                    _logger.LogInformation($"User {User.Identity.Name} View Bet other user: {email}");
                    ViewBag.canread = true;
                    userEmail = email;
                    ApplicationUser appUser = await _userManager.FindByEmailAsync(email);
                    if (appUser == null)
                    {
                        _logger.LogError($"-->User {User.Identity.Name} View Bet: User not found!");
                        return View("Error_404");
                    }
                    ViewBag.UserCanRead = appUser.FirstName.First().ToString().ToUpper() + appUser.FirstName.Substring(1) + " " + appUser.LastName.First().ToString().ToUpper() + appUser.LastName.Substring(1);
                }

                betUser = DbContext.Bets.Include(b => b.Match).Where(x => x.User.Email.Equals(userEmail)).OrderBy(o => o.Match.Id).ToList();
                newMyBetSesison = DbContext.FinalSessions.Where(x => x.User.Email.Equals(userEmail)).OrderBy(o => o.Ordinamento).ToList();

                if (betUser == null)
                    return RedirectToAction("Index", "Error", new { id = 403 });
                MyUserBetViewModel mub = null;
                if (betUser.Count + newMyBetSesison.Count == 0)
                {
                    //create new viewModel and save
                    _logger.LogInformation($"Create new bet for user: {userEmail}");
                    mub = await CreateNewVM(userEmail);
                }
                else
                {
                    _logger.LogInformation($"Load Bet from db for user: {userEmail}");
                    mub = new MyUserBetViewModel
                    {
                        BetUser = betUser,
                        BetUserSession = newMyBetSesison
                    };
                }
                if (mub != null) { return View(mub); }
                else { return View("Error"); }
            }
            catch (Exception e)
            {
                _logger.LogError($"--> Error in MyBet(Get) for User {User.Identity.Name}. Error Message: {e.Message}");
                return View("Error");
            }
        }
        [HttpPost]
        public async Task<IActionResult> MyBet(MyUserBetViewModel myUserBet)
        {
            try
            {

                ViewBag.canread = false;
                var user = User.Identity.Name;
                _logger.LogInformation($"MyBet Post: {user}");
                //IList<FinalSession> dbBetSessionUser = DbContext.FinalSessions.Where(x => x.User.Email.Equals(user)).ToList();
                //var sessionTeam = myUserBet.BetUserSession.ToList();
                if (Constants.EndFase3 > DateTime.Now.AddHours(+2))
                {
                    IList<Bet> betUser = new List<Bet>();
                    IList<FinalSession> newMyBetSesison = new List<FinalSession>();
                    bool esitoSave = await SaveBet(myUserBet);
                    //return RedirectToAction(nameof(MyBet));
                    if (esitoSave)
                    {
                        ViewBag.saveBet = true;
                        MyUserBetViewModel mub = new MyUserBetViewModel
                        {
                            BetUser = DbContext.Bets.Include(b => b.Match).Where(x => x.User.Email.Equals(user)).OrderBy(o => o.Match.Id).ToList(),
                            BetUserSession = DbContext.FinalSessions.Where(x => x.User.Email.Equals(user)).OrderBy(o => o.Ordinamento).ToList()
                        };

                        return View(mub);
                    }
                    else
                    {
                        ViewBag.saveBet = false;
                        //return View("Error");
                        MyUserBetViewModel mubFalseSalve = new MyUserBetViewModel
                        {
                            BetUser = DbContext.Bets.Include(b => b.Match).Where(x => x.User.Email.Equals(user)).OrderBy(o => o.Match.Id).ToList(),
                            BetUserSession = DbContext.FinalSessions.Where(x => x.User.Email.Equals(user)).OrderBy(o => o.Ordinamento).ToList()
                        };

                        return View(mubFalseSalve);
                    }
                }
                else
                {
                    ViewBag.saveBet = false;
                    //return View("Error");
                    MyUserBetViewModel mubFalseSalve = new MyUserBetViewModel
                    {
                        BetUser = DbContext.Bets.Include(b => b.Match).Where(x => x.User.Email.Equals(user)).OrderBy(o => o.Match.Id).ToList(),
                        BetUserSession = DbContext.FinalSessions.Where(x => x.User.Email.Equals(user)).OrderBy(o => o.Ordinamento).ToList()
                    };

                    return View(mubFalseSalve);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("--> Error in MyBet(Post)" + e.Message);
                return View("Error");
            }
        }

        public async Task<MyUserBetViewModel> CreateNewVM(string email)
        {
            try
            {
                //_logger.LogInformation("Start CreateNewVM");
                //var user = User.Identity.Name;
                ApplicationUser appUser = DbContext.Users.FirstOrDefault(x => x.Email.Equals(email));
                //var userLogins = _userManager.GetLoginsAsync(user);
                //var otherLogins = _signInManager.GetExternalAuthenticationSchemes().Where(auth => userLogins.All(ul => auth.AuthenticationScheme != ul.LoginProvider)).ToList();
                //ViewData["ShowRemoveButton"] = user.PasswordHash != null || userLogins.Count > 1;

                //costruisco Bet Fase 1 2 3
                var matchForBet = DbContext.Matchs.ToList();
                IList<Bet> newMyBet = new List<Bet>();
                foreach (var a in matchForBet)
                {
                    Bet b = new Bet
                    {
                        Date = a.Date,
                        BetIs = null,
                        EsitoBet = -1,
                        Match = a,
                        User = appUser,
                        Timestamp = DateTime.Now.AddHours(+2)

                    };
                    DbContext.Bets.Add(b);
                    newMyBet.Add(b);
                }

                //costruisco Bet Fase Session
                IList<FinalSession> newMyBetSesison = DbContext.FinalSessions.Where(x => x.User.Email.Equals(email)).OrderBy(o => o.Ordinamento).ToList();
                if (newMyBetSesison.Count == 0)
                {
                    newMyBetSesison = await AddNewSession(appUser);
                    foreach (var a in newMyBetSesison)
                        DbContext.FinalSessions.Add(a);
                }

                DbContext.SaveChanges();

                return new MyUserBetViewModel
                {
                    BetUser = newMyBet,
                    BetUserSession = newMyBetSesison
                };
            }
            catch (Exception e)
            {
                _logger.LogError("--> Error in CreateNewVM" + e.Message);
                return null;
            }

        }

        public async Task<IList<FinalSession>> AddNewSession(ApplicationUser appUser)
        {
            _logger.LogInformation($"Start AddNewSession: {appUser.Email}");
            IList<FinalSession> newMyBetSesison = new List<FinalSession>();
            //Ottavi
            newMyBetSesison.Add(new FinalSession()
            {
                User = appUser,
                Session = Constants.QUARTI,
                Ordinamento = 1,
                EsitoFS = -1,
                Timestamp = DateTime.Now.AddHours(+2)

            });
            newMyBetSesison.Add(new FinalSession()
            {
                User = appUser,
                Session = Constants.QUARTI,
                Ordinamento = 1,
                EsitoFS = -1,
                Timestamp = DateTime.Now.AddHours(+2)
            });
            newMyBetSesison.Add(new FinalSession()
            {
                User = appUser,
                Session = Constants.QUARTI,
                Ordinamento = 1,
                EsitoFS = -1,
                Timestamp = DateTime.Now.AddHours(+2)
            });
            newMyBetSesison.Add(new FinalSession()
            {
                User = appUser,
                Session = Constants.QUARTI,
                Ordinamento = 1,
                EsitoFS = -1,
                Timestamp = DateTime.Now.AddHours(+2)
            });
            newMyBetSesison.Add(new FinalSession()
            {
                User = appUser,
                Session = Constants.QUARTI,
                Ordinamento = 1,
                EsitoFS = -1,
                Timestamp = DateTime.Now.AddHours(+2)
            });
            newMyBetSesison.Add(new FinalSession()
            {
                User = appUser,
                Session = Constants.QUARTI,
                Ordinamento = 1,
                EsitoFS = -1,
                Timestamp = DateTime.Now.AddHours(+2)
            });
            newMyBetSesison.Add(new FinalSession()
            {
                User = appUser,
                Session = Constants.QUARTI,
                Ordinamento = 1,
                EsitoFS = -1,
                Timestamp = DateTime.Now.AddHours(+2)
            });
            newMyBetSesison.Add(new FinalSession()
            {
                User = appUser,
                Session = Constants.QUARTI,
                Ordinamento = 1,
                EsitoFS = -1,
                Timestamp = DateTime.Now.AddHours(+2)
            });
            //Quarti
            newMyBetSesison.Add(new FinalSession()
            {
                User = appUser,
                Session = Constants.SEMIFINALISTE,
                Ordinamento = 2,
                EsitoFS = -1,
                Timestamp = DateTime.Now.AddHours(+2)
            });
            newMyBetSesison.Add(new FinalSession()
            {
                User = appUser,
                Session = Constants.SEMIFINALISTE,
                Ordinamento = 2,
                EsitoFS = -1,
                Timestamp = DateTime.Now.AddHours(+2)
            });
            newMyBetSesison.Add(new FinalSession()
            {
                User = appUser,
                Session = Constants.SEMIFINALISTE,
                Ordinamento = 2,
                EsitoFS = -1,
                Timestamp = DateTime.Now.AddHours(+2)
            });
            newMyBetSesison.Add(new FinalSession()
            {
                User = appUser,
                Session = Constants.SEMIFINALISTE,
                Ordinamento = 2,
                EsitoFS = -1,
                Timestamp = DateTime.Now.AddHours(+2)
            });
            //Semifinaliste
            newMyBetSesison.Add(new FinalSession()
            {
                User = appUser,
                Session = Constants.FINALE,
                Ordinamento = 3,
                EsitoFS = -1,
                Timestamp = DateTime.Now.AddHours(+2)
            });
            newMyBetSesison.Add(new FinalSession()
            {
                User = appUser,
                Session = Constants.FINALE,
                Ordinamento = 3,
                EsitoFS = -1,
                Timestamp = DateTime.Now.AddHours(+2)
            });
            //Win
            newMyBetSesison.Add(new FinalSession()
            {
                User = appUser,
                Session = Constants.WIN,
                Ordinamento = 4,
                EsitoFS = -1,
                Timestamp = DateTime.Now.AddHours(+2)
            });

            return newMyBetSesison;
        }
        public async Task<bool> SaveBet(MyUserBetViewModel myUserBet)
        {
            try
            {
                bool isNotExF1 = Constants.EndFase1 > DateTime.Now.AddHours(+2);
                bool isNotExF2 = Constants.EndFase2 > DateTime.Now.AddHours(+2);
                bool isNotExF3 = Constants.EndFase3 > DateTime.Now.AddHours(+2);

                var user = User.Identity.Name;
                _logger.LogInformation($"Start SaveBet for user: {user}");
                bool ErrorBet = false;
                bool ErrorTeam = false;

                var BetIsList = myUserBet.BetUser.Where(x => x.BetIs != null).Select(x => x.BetIs).ToList();
                var bets = Constants.BetResults.Select(x => x.Value).ToList();
                if (BetIsList.Except(bets).Any())
                {
                    ErrorBet = true;
                    ModelState.AddModelError(String.Empty, "Errore nella selezione Bet");
                    _logger.LogInformation($"Inserito BetIs non corretto da user: {user} ");
                    _logger.LogInformation($"--> Errori nella form di salvataggio, user: {user}");
                    ViewBag.saveBet = false;
                    return false;
                }
                if (!ErrorBet)
                {
                    foreach (Bet bUser in myUserBet.BetUser)
                    {
                        Bet dbBetUser = DbContext.Bets.Include(b => b.Match).Where(x => x.User.Email.Equals(user) && x.Id == bUser.Id).FirstOrDefault();
                        int RoundNumber = dbBetUser.Match.RoundNumber;
                        if (RoundNumber == 1 && dbBetUser.Date>DateTime.Now.AddHours(+2))
                        {
                            dbBetUser.BetIs = bUser.BetIs;
                            dbBetUser.Timestamp = DateTime.Now.AddHours(+2);
                        }
                        if (RoundNumber == 2 && isNotExF2)
                        {
                            dbBetUser.BetIs = bUser.BetIs;
                            dbBetUser.Timestamp = DateTime.Now.AddHours(+2);
                        }
                        if (RoundNumber == 3 && isNotExF3)
                        {
                            dbBetUser.BetIs = bUser.BetIs;
                            dbBetUser.Timestamp = DateTime.Now.AddHours(+2);
                        }
                        //}

                    }
                }
                if (isNotExF2 && !ErrorBet)
                {
                    //ErrorTeam = false;
                    var LQ = myUserBet.BetUserSession.Where(x => x.Team != null && x.Session.Equals(Constants.QUARTI)).ToList();
                    var LS = myUserBet.BetUserSession.Where(x => x.Team != null && x.Session.Equals(Constants.SEMIFINALISTE)).ToList();
                    var LF = myUserBet.BetUserSession.Where(x => x.Team != null && x.Session.Equals(Constants.FINALE)).ToList();
                    var LW = myUserBet.BetUserSession.Where(x => x.Team != null && x.Session.Equals(Constants.WIN));

                    var teams = Constants.BetTeams.Select(x => x.Value).ToList();
                    //controlle che le squadrea inserite sono nelle 32 partecipanti. Controllo solo FQ in quanto le altre liste sono suoi sottoinsiemi
                    if (LQ.Count != 0 && LQ.Select(x => x.Team).Except(teams).Any())
                    {
                        ErrorTeam = true;
                        _logger.LogInformation($"Inserito Team non corretto da User: {user}");
                    }
                    //controllo che le squadre inserite sono contenute nella lista superiore

                    if (LS.Count != 0 && LS.Select(x => x.Team).Except(LQ.Select(x => x.Team)).Any())
                        ErrorTeam = true;
                    if (LF.Count != 0 && LF.Select(x => x.Team).Except(LS.Select(x => x.Team)).Any())
                        ErrorTeam = true;
                    if (LW != null && LW.Select(x => x.Team).Except(LF.Select(x => x.Team)).Any())
                        ErrorTeam = true;

                    if (ErrorTeam)
                    {
                        ModelState.AddModelError(String.Empty, "Errore nella selezione Team");
                        _logger.LogInformation($"Manipolato form Team da user: {user}");
                        _logger.LogInformation($"--> Errori nella form di salvataggio, user: {user}");
                        ViewBag.saveBet = false;
                        return false;

                    }
                    else
                    {
                        foreach (FinalSession fs in myUserBet.BetUserSession)
                        {
                            FinalSession fsDb = DbContext.FinalSessions.Where(x => x.User.Email.Equals(user) && x.Id == fs.Id).FirstOrDefault();
                            if (fs.Team != null)
                            {
                                fsDb.Team = fs.Team;
                            }
                            else
                            {
                                fsDb.Team = null;
                            }
                            fsDb.Timestamp = DateTime.Now.AddHours(+2);
                        }
                    }
                }

                DbContext.SaveChanges();
                ViewBag.saveBet = true;
                _logger.LogInformation($"Salvataggio effettuato correttamente, user: {user}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"--> Error in SaveBet: {ex.Message}");
                return false;
            }
        }
        [Authorize]
        public IActionResult Classifica()
        {
            try
            {
                //_logger.LogInformation("Start Classifica Action");
                ViewData["Message"] = "Your Classifica page.";
                //recupero classifica
                var user = User.Identity.Name;
                //Bet dbBetUser = DbContext.Bets.Include(b => b.Match).Where(x => x.User.Email.Equals(user) && x.Id == bUser.Id).FirstOrDefault();
                IList<Standings> StandList = DbContext.Standings.Include(u => u.User)
                    .OrderBy(x => x.Position)
                    .ToList();
                return View(new StandigsViewModel { StandingList = StandList });
            }
            catch (Exception ex)
            {
                _logger.LogError($"--> Error in Classifica: {ex.Message}");
                return View("Error");
            }
        }

        public async Task<IActionResult> Download()
        {
            try
            {
                string filename = "file\\Regolamento\\Betware_Regolamento.pdf";
                if (filename == null)
                    return Content("filename not present");

                var path = Path.Combine(
                               Directory.GetCurrentDirectory(),
                               "wwwroot", filename);

                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return File(memory, GetContentType(path), Path.GetFileName(path));
            }
            catch (Exception e)
            {
                _logger.LogError($"--> Exception Download file. Message: {e.Message} ");
                return null;
            }
        }
        public async Task<IActionResult> DownloadCalendario()
        {
            try
            {
                string filename = "file\\Calendario\\Calendario.pdf";
                if (filename == null)
                    return Content("filename not present");

                var path = Path.Combine(
                               Directory.GetCurrentDirectory(),
                               "wwwroot", filename);

                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return File(memory, GetContentType(path), Path.GetFileName(path));
            }
            catch (Exception e)
            {
                _logger.LogError($"--> Exception Download file. Message: {e.Message} ");
                return null;
            }
        }


        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                //{".txt", "text/plain"},
                {".pdf", "application/pdf"}
                //{".doc", "application/vnd.ms-word"},
                //{".docx", "application/vnd.ms-word"},
                //{".xls", "application/vnd.ms-excel"},
                //{".xlsx", "application/vnd.openxmlformats  officedocument.spreadsheetml.sheet"},  
                //{".png", "image/png"},
                //{".jpg", "image/jpeg"},
                //{".jpeg", "image/jpeg"},
                //{".gif", "image/gif"},
                //{".csv", "text/csv"}
            };
        }


        public IActionResult CalcolaClassifica()
        {
            try
            {
                _logger.LogInformation("Start CalcolaClassifica()");
                //ViewData["Message"] = "Your Classifica page.";
                //var user = User.Identity.Name;
                var allBet = DbContext.Bets.Include(b => b.Match).Include(u => u.User).Where(x => x.Match.Result != null).GroupBy(x => new { u = x.User.Email });
                //this.DbContext.Database.SetCommandTimeout(180);
                _logger.LogInformation($"Count List allBet {allBet.Count()}");
                List<Team> teamsFT = DbContext.Teams.Where(x => x.FT_Q == true).ToList();
                foreach (var userForBet in allBet)
                {
                    int risultatiIndovinati = 0;
                    int unoxdue = 0;
                    foreach (var ufb in userForBet)
                    {
                        if (ufb.BetIs != null)
                        {
                            //se BetIs è diverso da NULL procedo con il calcolo
                            var betIs = ufb.BetIs;
                            var pattern = "(-)";
                            string[] subSBetIs = Regex.Split(betIs, pattern);
                            var resMatch = ufb.Match.Result;
                            string[] subSResult = Regex.Split(resMatch, pattern);
                            bool betWithPoint = false;
                            if (subSResult[0].Trim().Equals(subSBetIs[0].Trim()) && subSResult[2].Trim().Equals(subSBetIs[2].Trim()))
                            {
                                //risultato indovinato
                                risultatiIndovinati++;
                                ufb.EsitoBet = 2;
                                betWithPoint = true;
                            }
                            else
                            {
                                var bet = Int32.Parse(subSBetIs[0]) - Int32.Parse(subSBetIs[2]);
                                var res = Int32.Parse(subSResult[0]) - Int32.Parse(subSResult[2]);
                                if ((bet > 0 && res > 0) || (bet == 0 && res == 0) || (bet < 0 && res < 0))
                                {
                                    unoxdue++;
                                    ufb.EsitoBet = 1;
                                    betWithPoint = true;
                                }
                            }
                            if (!betWithPoint)
                                ufb.EsitoBet = 0;
                        }
                       // DbContext.SaveChanges();
                    }
                    int countTeamQ = -1;
                    int countTeamS = -1;
                    int countTeamF = -1;
                    int countTeamW = -1;
                    Standings dbClassificaUser = DbContext.Standings.Where(x => x.User.Equals(userForBet.FirstOrDefault().User)).FirstOrDefault();

                    if (teamsFT.Count != 0)
                    {
                        _logger.LogInformation($"Calcolo la fase team. Count FT List: {teamsFT.Count}");
                        IList<FinalSession> fsListUser = DbContext.FinalSessions.Where(x => x.User.Equals(userForBet.FirstOrDefault().User)).ToList();
                        //IList<FinalSession> fs = fsList.Select(x => x.Team).Intersect(teams.Select(x => x.Name));
                        countTeamQ = teamsFT.Where(tft => tft.FT_Q == true && fsListUser.Where(x => x.Session.Equals(Constants.QUARTI)).Select(x => x.Team).Contains(tft.Name)).Count();
                        countTeamS = teamsFT.Where(tft => tft.FT_S == true && fsListUser.Where(x => x.Session.Equals(Constants.SEMIFINALISTE)).Select(x => x.Team).Contains(tft.Name)).Count();
                        countTeamF = teamsFT.Where(tft => tft.FT_F == true && fsListUser.Where(x => x.Session.Equals(Constants.FINALE)).Select(x => x.Team).Contains(tft.Name)).Count();
                        countTeamW = teamsFT.Where(tft => tft.FT_W == true && fsListUser.Where(x => x.Session.Equals(Constants.WIN)).Select(x => x.Team).Contains(tft.Name)).Count();

                        dbClassificaUser.FT_Q = countTeamQ;
                        dbClassificaUser.FT_S = countTeamS;
                        dbClassificaUser.FT_F = countTeamF;
                        dbClassificaUser.FT_W = countTeamW;
                        dbClassificaUser.Punti = unoxdue + risultatiIndovinati * 3 + countTeamQ * 2 + countTeamS * 4 + countTeamF * 6 + countTeamW * 10;
                    }
                    else
                    {
                        dbClassificaUser.Punti = risultatiIndovinati * 3 + unoxdue;
                    }
                    dbClassificaUser.UnoxDue = unoxdue;
                    dbClassificaUser.RisultatiEsati = risultatiIndovinati;

                    
                }
                DbContext.SaveChanges();
                int posizione = 1;
                IList<Standings> dbAllClassif = DbContext.Standings.Include(u => u.User)
                    .OrderByDescending(p => p.Punti)
                    .ThenByDescending(r => r.RisultatiEsati)
                    .ThenByDescending(x => x.UnoxDue)
                    .ThenByDescending(w => w.FT_W)
                    .ThenByDescending(f => f.FT_F)
                    .ThenByDescending(s => s.FT_S)
                    .ThenByDescending(q => q.FT_Q)
                    .ThenBy(l => l.User.LastName)
                    .ThenBy(l => l.User.FirstName)
                    .ToList();
                //foreach (var c in dbAllClassif)
                //{

                //    c.Position = posizione;
                //    posizione++;
                //}

                for (int i = 0; i < dbAllClassif.Count; i++)
                {
                    if (i != 0)
                    {
                        if (dbAllClassif[i - 1].RisultatiEsati != dbAllClassif[i].RisultatiEsati ||
                            dbAllClassif[i - 1].UnoxDue != dbAllClassif[i].UnoxDue ||
                            dbAllClassif[i - 1].FT_Q != dbAllClassif[i].FT_Q ||
                            dbAllClassif[i - 1].FT_S != dbAllClassif[i].FT_S ||
                            dbAllClassif[i - 1].FT_F != dbAllClassif[i].FT_F ||
                            dbAllClassif[i - 1].FT_W != dbAllClassif[i].FT_W)
                            posizione++;
                    }
                    dbAllClassif[i].Position = posizione;
                }

                //setto per ogni team della FinalSession, il volore 1 in corrispondenza del team indovinato
                IList<FinalSession> FSLQ = DbContext.FinalSessions.Where(x => x.Session.Equals(Constants.QUARTI)).ToList();
                IList<FinalSession> FSListQ = FSLQ.Where(b => teamsFT.Any(a => a.Name == b.Team)).ToList();
                foreach (var fsq in FSListQ)
                {
                    fsq.EsitoFS = 1;
                }
                IList<FinalSession> FSLS = DbContext.FinalSessions.Where(x => x.Session.Equals(Constants.SEMIFINALISTE)).ToList();
                IList<FinalSession> FSListS = FSLS.Where(b => teamsFT.Where(x => x.FT_S == true).Any(a => a.Name == b.Team)).ToList();
                foreach (var fss in FSListS)
                {
                    fss.EsitoFS = 1;
                }
                IList<FinalSession> FSLF = DbContext.FinalSessions.Where(x => x.Session.Equals(Constants.FINALE)).ToList();
                IList<FinalSession> FSListF = FSLF.Where(b => teamsFT.Where(x => x.FT_F == true).Any(a => a.Name == b.Team)).ToList();
                foreach (var fsf in FSListF)
                {
                    fsf.EsitoFS = 1;
                }
                IList<FinalSession> FSLW = DbContext.FinalSessions.Where(x => x.Session.Equals(Constants.WIN)).ToList();
                IList<FinalSession> FSListW = FSLW.Where(b => teamsFT.Where(x => x.FT_W == true).Any(a => a.Name == b.Team)).ToList();
                foreach (var fsw in FSListW)
                {
                    fsw.EsitoFS = 1;
                }
                DbContext.SaveChanges();
                return View("Classifica", new StandigsViewModel { StandingList = dbAllClassif });
            }
            catch (DbUpdateException ex)
            {
                // Logga l'errore dettagliato
                //Console.WriteLine($"Errore durante il salvataggio: {ex.InnerException?.Message ?? ex.Message}");
                _logger.LogInformation($"Errore durante il salvataggio: {ex.InnerException?.Message ?? ex.Message}");
                if (ex.InnerException != null)
                {
                    //Console.WriteLine($"Dettagli dell'inner exception: {ex.InnerException.Message}");
                    _logger.LogInformation($"Dettagli dell'inner exception: {ex.InnerException.Message}");
                    if (ex.InnerException.InnerException != null)
                    {
                        //Console.WriteLine($"Dettagli dell'inner-inner exception: {ex.InnerException.InnerException.Message}");
                        _logger.LogInformation($"Dettagli dell'inner-inner exception: {ex.InnerException.InnerException.Message}");
                    }
                }
                return View("Error");
            }
            catch (Exception ex)
            {
                // Logga altri errori generici
                _logger.LogInformation($"Errore generico: {ex.Message}");
                return View("Error");
            }
        }
        public IActionResult Error()
        {
            return View();
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }


    }
}

