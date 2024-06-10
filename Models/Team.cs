using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Betware.Models
{
    public class Team
    {

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool FT_Q { get; set; }
        public bool FT_S { get; set; }
        public bool FT_F { get; set; }
        public bool FT_W { get; set; }


    }
}
