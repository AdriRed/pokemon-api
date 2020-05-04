using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokedexAPI.Models
{
    public class LoginResult : UserModel
    {
        public string Token { get; set; }
    }
}
