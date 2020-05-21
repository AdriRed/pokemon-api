using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokedexAPI.Models;
using PokedexAPI.Models.Extensions;
using PokedexAPI.Services;

namespace PokedexAPI.Controllers
{
    /// <summary>
    /// Pokemon like actions
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        IPokemonService _service;
        public PokemonController(IPokemonService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get your favourite pokemon
        /// </summary>
        /// <returns></returns>
        [HttpGet("favourites")]
        [ProducesResponseType(typeof(ICollection<FavouritePokemonModel>), 200)]
        [ProducesResponseType(typeof(ErrorModel), 500)]
        public async Task<ICollection<FavouritePokemonModel>> GetFavourites()
        {
            var list = await _service.GetAllFavouritesAsync(Convert.ToInt32(this.User.Identity.Name));
            return list.Select(x => x.ToModel()).ToList();
        }

        /// <summary>
        /// Get your custom pokemon
        /// </summary>
        /// <returns></returns>
        [HttpGet("custom/self")]
        [ProducesResponseType(typeof(ICollection<CustomPokemonModel>), 200)]
        [ProducesResponseType(typeof(ErrorModel), 500)]
        public async Task<ICollection<CustomPokemonModel>> GetCustomSelf()
        {
            var list = await _service.GetAllCustomAsync(Convert.ToInt32(this.User.Identity.Name));
            return list.Select(x => x.ToModel()).ToList();
        }

        /// <summary>
        /// Get the custom pokemon of other user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("custom/from/{id}")]
        [ProducesResponseType(typeof(ICollection<CustomPokemonModel>), 200)]
        [ProducesResponseType(typeof(ErrorModel), 500)]
        public async Task<ICollection<CustomPokemonModel>> GetCustomOther(int id)
        {
            var list = await _service.GetAllCustomAsync(id);
            return list.Select(x => x.ToModel()).ToList();
        }

        /// <summary>
        /// Get single custom pokemon
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("custom/{id}")]
        [ProducesResponseType(typeof(CustomPokemonModel), 200)]
        [ProducesResponseType(typeof(ErrorModel), 500)]
        public async Task<CustomPokemonModel> GetCustom(int id)
        {
            var x = await _service.GetCustomAsync(id);
            return x.ToModel();
        }

        /// <summary>
        /// Adds a custom pokemon for your list
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("custom")]
        [ProducesResponseType(typeof(CustomPokemonModel), 200)]
        [ProducesResponseType(typeof(ErrorModel), 500)]
        public async Task<CustomPokemonModel> AddCustom([FromBody]CustomPokemonModel model)
        {
            var x = await _service.AddCustom(Convert.ToInt32(this.User.Identity.Name), model);
            return x.ToModel();
        }

        /// <summary>
        /// Adds a favourite to your favourite pokemon list
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("favourites")]
        [ProducesResponseType(typeof(FavouritePokemonModel), 200)]
        [ProducesResponseType(typeof(ErrorModel), 500)]
        public async Task<FavouritePokemonModel> AddFavourite([FromBody]FavouritePokemonModel model)
        {
            var x = await _service.AddFavourite(Convert.ToInt32(this.User.Identity.Name), model);
            return x.ToModel();
        }

        /// <summary>
        /// Deletes a favourite from your favourite pokemon list
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("favourites/{id}")]
        [ProducesResponseType(typeof(FavouritePokemonModel), 200)]
        [ProducesResponseType(typeof(ErrorModel), 500)]
        public async Task<FavouritePokemonModel> DeleteFavourite(int id)
        {
            var x = await _service.RemoveFavourite(Convert.ToInt32(this.User.Identity.Name), id);
            return x.ToModel();
        }

        /// <summary>
        /// Deletes a custom pokemon from your custom pokemon list
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("custom/{id}")]
        [ProducesResponseType(typeof(CustomPokemonModel), 200)]
        [ProducesResponseType(typeof(ErrorModel), 500)]
        public async Task<CustomPokemonModel> DeleteCustom(int id)
        {
            var x = await _service.RemoveCustom(Convert.ToInt32(this.User.Identity.Name), id);
            return x.ToModel();
        }
    }
}