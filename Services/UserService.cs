using FinalAutoFillServer.Data;
using FinalAutoFillServer.Entities;
using FinalAutoFillServer.Helpers;
using FinalAutoFillServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace FinalAutoFillServer.Services
{
    public class UserService : IUserRepository
    {
        private AutoFillContext _context;

        public UserService(AutoFillContext context)
        {
            _context = context;
        }

        public User Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;
            var user = _context.Users.SingleOrDefault(x => x.UserName == username);
            //check if username exists
            if (user == null)
                return null;
            //check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;
            return user;
        }
        public User Create(User user, string password, string origin)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");
            if (_context.Users.Any(x => x.UserName == user.UserName))
                throw new AppException("Username \"" + user.UserName + "\" is already taken");
            if (_context.Users.Any(x => x.Email == user.Email))
                throw new AppException("Email \"" + user.Email + "\" is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            if (string.IsNullOrWhiteSpace(user.Role)) user.Role = Role.Customer;

            user.Created = DateTime.Now;
            user.VerificationToken = randomTokenString();

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        private string randomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public User GetById(int id)
        {
            return _context.Users.Find(id);
        }

        public void Update(User userParam, string password = null)
        {
            var user = _context.Users.Find(userParam.UserId);

            if (user == null)
                throw new AppException("User not found");
            if (!string.IsNullOrWhiteSpace(userParam.UserName) && userParam.UserName != user.UserName)
            {
                //update username if it has changed
                if (_context.Users.Any(x => x.UserName == userParam.UserName))
                    throw new AppException("Username " + userParam.UserName + " is already taken");
                user.UserName = userParam.UserName;
            }
            //update password if provied
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            user.Updated = DateTime.Now;

            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public bool CheckUser(int userId)
        {

            throw new NotImplementedException();
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

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

        public void UpdateToken(User user, string currentToken)
        {
            user.CurrentToken = currentToken;
            user.Updated = DateTime.Now;
            _context.Users.Update(user);
            _context.SaveChanges();
        }

    }
}
