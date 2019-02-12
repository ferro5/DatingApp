using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        public UserService(DataContext context)
        {
            _context = context;

        }

        #region Authenticate
        public async Task<User> Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) return null;
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == username);
            if (user == null) return null;
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;
            return user;
        }



        #endregion

        private static bool VerifyPasswordHash(string password, byte[] userPasswordHash, byte[] userPasswordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (userPasswordHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (userPasswordSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(userPasswordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != userPasswordHash[i]) return false;
                }
            }

            return true;
        }

        #region Create

        public async Task<User> Create(User user, string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new AppException("Password is required");
            if (await _context.Users.AnyAsync(x => x.UserName == user.UserName))
                throw new AppException("Username\"" + user.UserName + "\" is already taken");
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        #endregion

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password==null)throw new ArgumentNullException("password");
            if (string.IsNullOrEmpty(password))throw new ArgumentException("Password cannot be empty string ","password");
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
          
           
        }
        #region Delete

        public void Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }
        #endregion

        #region GetAll
        public IEnumerable<User> GetAll()
        {
            return _context.Users;
        }


        #endregion

        #region GetById

        public async Task<User> GetById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        #endregion


        public void Update(User userparam, string password = null)
        {
            var user = _context.Users.Find(userparam.Id);
            if (user==null)throw new Exception("User not found");
            if (userparam.UserName != user.UserName)
            {
                if (_context.Users.Any(x=>x.UserName==user.UserName))
                
                    throw new AppException("Username" + userparam.UserName + "already taken");
                
            }

            user.FirstName = userparam.FirstName;
            user.LastName = userparam.LastName;
            user.UserName = userparam.UserName;

            if (string.IsNullOrEmpty(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password,out passwordHash,out passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            _context.Users.Update(user);
            _context.SaveChanges();
        }
    }
}
