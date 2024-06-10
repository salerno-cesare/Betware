using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Betware.Models
{
    public class FinalSession
    {
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        
        public string Session { get; set; }
        [RequiredTeam]
        public string Team { get; set; }
        public int Ordinamento { get; set; }
        public int EsitoFS { get; set; }
        public DateTime Timestamp { get; set; }

        public class RequiredTeam : ValidationAttribute
        {

            public RequiredTeam()
            {
            }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                FinalSession fs = (FinalSession)validationContext.ObjectInstance;

                if (fs.Team != null) return ValidationResult.Success;
                else
                    return new ValidationResult("Team is required.");

            }
        }

    }
}
