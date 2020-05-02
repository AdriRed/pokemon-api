using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokedexAPI.Models
{
    public class UserModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public byte[] Photo { get; set; }
    }
}
