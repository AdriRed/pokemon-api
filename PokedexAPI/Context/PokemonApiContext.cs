using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PokedexAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokedexAPI.Context
{
    public class PokemonApiContext : DbContext
    {
        public PokemonApiContext(DbContextOptions options) : base(options)
        {
        }

        protected PokemonApiContext()
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<FavouritePokemon> FavouritePokemons { get; set; }
        public DbSet<CustomPokemon> CustomPokemons { get; set; }
    }
}
