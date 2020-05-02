using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokedexAPI.Models
{
    public class ChangePasswordModel
    {
        public string Password { get; set; }
        public string RepeatPassword { get; set; }
    }
}
