﻿using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PokedexAPI.Context;
using PokedexAPI.Entities;
using PokedexAPI.Extensions;
using PokedexAPI.Helpers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PokedexAPI.Services
{
    public interface IUserService
    {
        Task<User> AuthenticateAsync(string email, string password);
        IEnumerable<User> GetAll();
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByIdAsync(int id);
        Task<User> CreateAsync(User user, string password);
        Task<User> UpdateAsync(User user, string password = null, string repeatPassword = null);
        Task<User> DeleteAsync(string email);
    }

    public class UserService : IUserService
    {
        private readonly PokemonApiContext _context;
        private readonly AppSettings _appSettings;

        public UserService(PokemonApiContext context, AppSettings appSettings)
        {
            _context = context;
            _appSettings = appSettings;
        }

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

            // return null if user not found
            if (user == null)
                throw new PokemonAPIException("User not found", ExceptionConstants.USER_NOT_FOUND);

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                throw new PokemonAPIException("Password not valid", ExceptionConstants.BAD_LOGIN);

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            return user;
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User> CreateAsync(User user, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(user.Email))
                throw new PokemonAPIException("Email is required", ExceptionConstants.BAD_REGISTER);

            if (string.IsNullOrWhiteSpace(user.UserName))
                throw new PokemonAPIException("Username is required", ExceptionConstants.BAD_REGISTER);

            if (string.IsNullOrWhiteSpace(password))
                throw new PokemonAPIException("Password is required", ExceptionConstants.BAD_REGISTER);

            if (await GetByEmailAsync(user.Email) != null)
                throw new PokemonAPIException("Email \"" + user.Email + "\" is already taken", ExceptionConstants.BAD_REGISTER);

            if (_context.Users.Any(x => x.UserName == user.UserName))
                throw new PokemonAPIException("Username \"" + user.UserName + "\" is already taken", ExceptionConstants.BAD_REGISTER);

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(user);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }

            return user;
        }

        public async Task<User> UpdateAsync(User userParam, string password = null, string repeatPassword = null)
        {
            var user = _context.Users.Find(userParam.Id);

            if (user == null)
                throw new PokemonAPIException("User not found", ExceptionConstants.USER_NOT_FOUND);

            // update username if it has changed
            if (!string.IsNullOrWhiteSpace(userParam.Email) && userParam.Email != user.Email)
            {
                // throw error if the new username is already taken
                if (await GetByEmailAsync(user.Email) != null)
                    throw new PokemonAPIException("Email is required", ExceptionConstants.BAD_REGISTER);

                user.Email = userParam.Email;
            }

            if (userParam.Photo != null && Enumerable.SequenceEqual(userParam.Photo, user.Photo))
            {
                user.Photo = userParam.Photo;
            }

            // update password if provided
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                if (password != repeatPassword)
                    throw new PokemonAPIException("Passwords dont match", ExceptionConstants.PASSWORDS_DONT_MATCH);

                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            _context.Users.Update(user);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
            return user;
        }

        public async Task<User> DeleteAsync(string email)
        {
            var user = await GetByEmailAsync(email);
            _context.Users.Remove(user);
            if (user != null)
            {
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return user;
        }

        // private helper methods

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new PokemonAPIException("Value cannot be empty or whitespace only string.", ExceptionConstants.BAD_LOGIN);

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new PokemonAPIException("Value cannot be empty or whitespace only string.", ExceptionConstants.BAD_LOGIN);

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }
    }
}
