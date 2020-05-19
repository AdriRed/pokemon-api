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

        [HttpGet("favourites")]
        public async Task<ICollection<FavouritePokemonModel>> GetFavourites()
        {
            var list = await _service.GetAllFavouritesAsync(Convert.ToInt32(this.User.Identity.Name));
            return list.Select(x => x.ToModel()).ToList();
        }

        [HttpGet("custom/self")]
        public async Task<ICollection<CustomPokemonModel>> GetCustomSelf()
        {
            var list = await _service.GetAllCustomAsync(Convert.ToInt32(this.User.Identity.Name));
            return list.Select(x => x.ToModel()).ToList();
        }

        [HttpGet("custom/from/{id}")]
        public async Task<ICollection<CustomPokemonModel>> GetCustomOther(int id)
        {
            var list = await _service.GetAllCustomAsync(id);
            return list.Select(x => x.ToModel()).ToList();
        }

        [HttpGet("custom/{id}")]
        public async Task<CustomPokemonModel> GetCustom(int id)
        {
            var x = await _service.GetCustomAsync(id);
            return x.ToModel();
        }

        [HttpPost("custom")]
        public async Task<CustomPokemonModel> AddCustom([FromBody]CustomPokemonModel model)
        {
            var x = await _service.AddCustom(Convert.ToInt32(this.User.Identity.Name), model);
            return x.ToModel();
        }

        [HttpPost("favourites")]
        public async Task<FavouritePokemonModel> AddFavourite([FromBody]FavouritePokemonModel model)
        {
            var x = await _service.AddFavourite(Convert.ToInt32(this.User.Identity.Name), model);
            return x.ToModel();
        }

        [HttpDelete("favourites/{id}")]
        public async Task<FavouritePokemonModel> DeleteFavourite(int id)
        {
            var x = await _service.RemoveFavourite(Convert.ToInt32(this.User.Identity.Name), id);
            return x.ToModel();
        }

        [HttpDelete("custom/{id}")]
        public async Task<CustomPokemonModel> DeleteCustom(int id)
        {
            var x = await _service.RemoveCustom(Convert.ToInt32(this.User.Identity.Name), id);
            return x.ToModel();
        }
    }
}