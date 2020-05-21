using Microsoft.EntityFrameworkCore;
using PokedexAPI.Context;
using PokedexAPI.Entities;
using PokedexAPI.Models;
using PokedexAPI.Models.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace PokedexAPI.Services
{
    public interface IPokemonService
    {
        Task<ICollection<FavouritePokemon>> GetAllFavouritesAsync(int id);
        Task<FavouritePokemon> AddFavourite(int id, FavouritePokemonModel model);
        Task<FavouritePokemon> RemoveFavourite(int id, int pokemonId);

        Task<ICollection<CustomPokemon>> GetAllCustomAsync(int id);
        Task<CustomPokemon> GetCustomAsync(int id);
        Task<CustomPokemon> AddCustom(int id, CustomPokemonModel model);
        Task<CustomPokemon> RemoveCustom(int id, int custom);

    }
    public class PokemonService : IPokemonService
    {
        private readonly PokemonApiContext _context;

        public PokemonService(PokemonApiContext context)
        {
            _context = context;
        }


        public async Task<CustomPokemon> AddCustom(int id, CustomPokemonModel model)
        {
            var entity = model.ToEntity();

            entity.UserId = id;

            await _context.CustomPokemons.AddAsync(entity);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }

            return entity;

        }

        public async Task<FavouritePokemon> AddFavourite(int id, FavouritePokemonModel model)
        {
            var entity = model.ToEntity();

            entity.UserId = id;
            var user = await _context.Users.FindAsync(id);
            if (user.FavouritePokemons.Any(x => x.PokemonId == model.FavouritePokemonId))
                throw new PokemonAPIException("This pokemon already exists!", ExceptionConstants.BAD_REQUEST);

            await _context.FavouritePokemons.AddAsync(entity);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }

            return entity;
        }

        public async Task<ICollection<CustomPokemon>> GetAllCustomAsync(int id)
        {
            var user = await _context.Users.Include(x => x.CustomPokemons).FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
                throw new PokemonAPIException("This user dont exists", ExceptionConstants.BAD_REQUEST);
            return user.CustomPokemons.ToList();
        }

        public async Task<ICollection<FavouritePokemon>> GetAllFavouritesAsync(int id)
        {
            var user = await _context.Users.Include(x => x.FavouritePokemons).FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
                throw new PokemonAPIException("This user dont exists", ExceptionConstants.BAD_REQUEST);
            return user.FavouritePokemons.ToList();
        }

        public async Task<CustomPokemon> GetCustomAsync(int id)
        {
            return await _context.CustomPokemons.FindAsync(id);
        }

        public async Task<CustomPokemon> RemoveCustom(int id, int custom)
        {
            var user = await _context.Users.Include(x => x.CustomPokemons).FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
                throw new PokemonAPIException("This user dont exists", ExceptionConstants.BAD_REQUEST);
            var poke = user.CustomPokemons.FirstOrDefault(x => x.Id == custom);
            if (poke == null)
                throw new PokemonAPIException("This pokemon dont exists!", ExceptionConstants.BAD_REQUEST);

            _context.Remove(poke);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }

            return poke;
        }

        public async Task<FavouritePokemon> RemoveFavourite(int id, int pokemonId)
        {
            var user = await _context.Users.Include(x => x.FavouritePokemons).FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
                throw new PokemonAPIException("This user dont exists", ExceptionConstants.BAD_REQUEST);
            var poke = user.FavouritePokemons.FirstOrDefault(x => x.PokemonId == pokemonId);
            if (poke == null)
                throw new PokemonAPIException("This pokemon dont exists!", ExceptionConstants.BAD_REQUEST);

            _context.Remove(poke);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }

            return poke;

        }
    }
}
