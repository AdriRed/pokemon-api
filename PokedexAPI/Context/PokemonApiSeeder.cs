using PokedexAPI.Entities;
using PokedexAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokedexAPI.Context
{
    public class PokemonApiSeeder
    {
        private readonly PokemonApiContext _context;
        private readonly IUserService _userService;

        public PokemonApiSeeder(PokemonApiContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task Seed()
        {
            _context.Database.EnsureCreated();
            if (!_context.Users.Any())
            {
                var user = new User
                {
                    Email = "test@test.com",
                    UserName = "test"
                };
                await _userService.CreateAsync(user, "test", "test");
            }
        }
    }
}
