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

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]Login model)
        {
            var user = await _userService.AuthenticateAsync(model.Email, model.Password);

            return Ok(user.ToLoginResult());
        }

        [AllowAnonymous]
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

        // GET: api/Account/self
        [HttpGet("self")]
        public async Task<IActionResult> Self()
        {
            var user = await _userService.GetByIdAsync(Convert.ToInt32(User.Identity.Name));
            return Ok(user.ToModel());
        }

        // GET: api/Account/edit
        [HttpPut("edit")]
        public async Task<IActionResult> Edit([FromBody]UserModel model)
        {
            var user = await _userService.GetByIdAsync(Convert.ToInt32(User.Identity.Name));
            user = await _userService.UpdateAsync(model.ToEntity());
            return Ok(user.ToModel());
        }

        // POST: api/Account/edit
        [HttpPut("changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordModel model)
        {
            var user = await _userService.GetByIdAsync(Convert.ToInt32(User.Identity.Name));
            user = await _userService.UpdateAsync(user, model.Password, model.RepeatPassword);
            return Ok(user.ToModel());
        }
    }
}
