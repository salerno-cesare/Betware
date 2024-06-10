using Betware.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Betware.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Match> Matchs { get; set; }
        public virtual DbSet<Bet> Bets { get; set; }
        public virtual DbSet<Standings> Standings { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<FinalSession> FinalSessions { get; set; }
    }
}
