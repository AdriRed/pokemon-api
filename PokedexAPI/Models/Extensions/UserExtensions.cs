using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokedexAPI.Models.Extensions
{
    public static class UserExtensions
    {
        public static UserModel ToModel(this Entities.User user)
        {
            return new UserModel
            {
                Email = user.Email,
                Photo = user.Photo,
                UserName = user.UserName
            };
        }

        public static LoginResult ToLoginResult(this Entities.User user)
        {
            return new LoginResult
            {
                UserName = user.UserName,
                Email = user.Email,
                Photo = user.Photo,
                Token = user.Token
            };
        }

        public static Entities.User ToEntity(this UserModel user)
        {
            return new Entities.User
            {
                Email = user.Email,
                Photo = user.Photo,
                UserName = user.UserName
            };
        }
    }
}
