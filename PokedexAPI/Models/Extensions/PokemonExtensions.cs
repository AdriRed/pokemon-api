using PokedexAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokedexAPI.Models.Extensions
{
    public static class PokemonExtensions
    {
        public static FavouritePokemonModel ToModel (this FavouritePokemon entity)
        {
            return new FavouritePokemonModel
            {
                Id = entity.Id,
                FavouritePokemonId = entity.PokemonId
            };
        }

        public static FavouritePokemon ToEntity (this FavouritePokemonModel model)
        {
            return new FavouritePokemon
            {
                Id = model.Id,
                PokemonId = model.FavouritePokemonId
            };
        }

        public static CustomPokemonModel ToModel(this CustomPokemon entity)
        {
            return new CustomPokemonModel
            {
                Id = entity.Id,
                Photo = entity.Image,
                Name = entity.Name
            };
        }

        public static CustomPokemon ToEntity(this CustomPokemonModel model)
        {
            return new CustomPokemon
            {
                Id = model.Id,
                Image = model.Photo,
                Name = model.Name
            };
        }
    }
}
