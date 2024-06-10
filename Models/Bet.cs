using System;
using System.ComponentModel.DataAnnotations;

namespace Betware.Models
{
    public class Bet
    {
        public int Id { get; set; }
        
        public DateTime Date { get; set; }
        public DateTime Timestamp { get; set; }
        //fild BetIs is the user's bet
        [RequiredBet]
        public string BetIs { get; set; }
        
        public Match Match { get; set; }
        public int EsitoBet { get; set; }

        public ApplicationUser User { get; set; }
    }

    public class RequiredBet : ValidationAttribute
    {

        public RequiredBet()
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Bet bet = (Bet)validationContext.ObjectInstance;

            if (bet.BetIs != null) return ValidationResult.Success;
            else
                return new ValidationResult("BetIs is required.");
            
        }
    }
}
