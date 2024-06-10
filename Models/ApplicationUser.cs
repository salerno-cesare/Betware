using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Betware.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class

    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public bool AccessToApp { get; set; }
        public DateTime Timestamp { get; set; }

        public ICollection<Bet> Bets { get; set; }
    }


}
