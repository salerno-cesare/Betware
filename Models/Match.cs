using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Betware.Models
{
    public class Match
    {
  
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Result { get; set; }
        public string Location { get; set; }
        public int RoundNumber { get; set; }
        public string Group { get; set; }
        public string HomeT { get; set; }
        public string AwayT { get; set; }
        public ICollection<Bet> Bets { get; set; }

    }
}
