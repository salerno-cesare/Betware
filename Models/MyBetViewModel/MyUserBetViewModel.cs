using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Betware.Models.MyBetViewModel
{
    public class MyUserBetViewModel
    {
        //public ApplicationUser User { get; set; }
        public IList<Bet> BetUser { get; set; }
        public IList<FinalSession> BetUserSession { get; set; }
    }
}
