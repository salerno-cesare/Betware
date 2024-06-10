using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betware.Models
{
    public class Standings
    {
        public int Id { get; set; }
        public int Position { get; set; }
        public int RisultatiEsati { get; set; }
        public int UnoxDue { get; set; }
        public int FT_Q { get; set; }
        public int FT_S { get; set; }
        public int FT_F { get; set; }
        public int FT_W { get; set; }
        public int Punti { get; set; }
        public ApplicationUser User { get; set; }

    }
}
