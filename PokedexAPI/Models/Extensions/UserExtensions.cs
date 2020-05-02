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
