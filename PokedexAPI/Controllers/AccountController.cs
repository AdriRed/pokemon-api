using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Scaffolding;
using PokedexAPI.Extensions;
using PokedexAPI.Models;
using PokedexAPI.Models.Extensions;
using PokedexAPI.Services;

namespace PokedexAPI.Controllers
{   
    /// <summary>
    /// Controller of account actions
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Login user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResult), 200)]
        [ProducesResponseType(typeof(ErrorModel), 500)]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]Login model)
        {
            var user = await _userService.AuthenticateAsync(model.Email, model.Password);

            return Ok(user.ToLoginResult());
        }

        /// <summary>
        /// Register user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResult), 200)]
        [ProducesResponseType(typeof(ErrorModel), 500)]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterUser model)
        {
            var user = await _userService.CreateAsync(new Entities.User()
            {
                Email = model.Email,
                UserName = model.Username
            }, model.Password, model.RepeatPassword);

            user = await _userService.AuthenticateAsync(model.Email, model.Password);

            return Ok(user.ToLoginResult());
        }

        /// <summary>
        /// Get self data
        /// </summary>
        /// <returns></returns>
        // GET: api/Account/self
        [HttpGet("self")]
        [ProducesResponseType(typeof(UserModel), 200)]
        [ProducesResponseType(typeof(ErrorModel), 500)]
        public async Task<IActionResult> Self()
        {
            var user = await _userService.GetByIdAsync(Convert.ToInt32(User.Identity.Name));
            return Ok(user.ToModel());
        }

        /// <summary>
        /// Sets the photo icon for yourself
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        // PUT: api/Account/setphoto
        [HttpPut("setphoto")]
        [ProducesResponseType(typeof(UserModel), 200)]
        [ProducesResponseType(typeof(ErrorModel), 500)]
        public async Task<IActionResult> SetPhoto([FromForm]PhotoModel model)
        {
            var user = await _userService.SetPhotoAsync(Convert.ToInt32(User.Identity.Name), model.File);
            return Ok(user.ToModel());
        }

        /// <summary>
        /// Edits yourself
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        // GET: api/Account/edit
        [HttpPut("edit")]
        [ProducesResponseType(typeof(UserModel), 200)]
        [ProducesResponseType(typeof(ErrorModel), 500)]
        public async Task<IActionResult> Edit([FromBody]UserModel model)
        {
            var user = await _userService.UpdateAsync(Convert.ToInt32(this.User.Identity.Name), model.ToEntity());
            return Ok(user.ToModel());
        }

        /// <summary>
        /// Changes your password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        // POST: api/Account/edit
        [HttpPut("changepassword")]
        [ProducesResponseType(typeof(UserModel), 200)]
        [ProducesResponseType(typeof(ErrorModel), 500)]
        public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordModel model)
        {
            var user = await _userService.UpdateAsync(Convert.ToInt32(User.Identity.Name), null, model.Password, model.RepeatPassword);
            return Ok(user.ToModel());
        }
    }
}
