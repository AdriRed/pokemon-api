using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokedexAPI.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }

        public byte[] Photo { get; set; }

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public string Token { get; set; }

        public virtual ICollection<FavouritePokemon> FavouritePokemons { get; set; } = new List<FavouritePokemon>();
    }
}
