using PokedexAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokedexAPI.Extensions
{
    public static class UserExtensions
    {
        public static User WithoutPassword(this User user)
        {
            user.PasswordHash = null;
            user.PasswordHash = null;
            return user;
        }

        public static User WithoutToken(this User user)
        {
            user.Token = null;
            return user;
        }

        public static ICollection<User> WithoutPassword(this ICollection<User> users)
        {
            return users.Select(x => x.WithoutPassword()).ToList();
        }

        public static ICollection<User> WithoutToken(this ICollection<User> users)
        {
            return users.Select(x => x.WithoutToken()).ToList();
        }
    }
}
