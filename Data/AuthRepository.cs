using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace battleship_server.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _dataContext;

        public AuthRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Task<ServiceResponse<string>> Login(string username, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            var response = new ServiceResponse<int>();

            // Check if user already existed
            if (await UserExists(user.Username)){
                response.Success = false;
                response.Message = "Username already existed!";
                return response;
            }

            // Else perform register
            // Call the CreateHash method to generate the hashed password
            CreateHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            // Setting the password to the user obj
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;


            _dataContext.Users.Add(user);
            await _dataContext.SaveChangesAsync();

            
            response.Data = user.Id;

            return response;
        }

        public async Task<bool> UserExists(string username)
        {
            if (await _dataContext.Users.AnyAsync(u => u.Username.ToLower() == username.ToLower()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        // Method for hashing the password
        private void CreateHash(string plainPassword, out byte[] passwordHash, out byte[] passwordSalt)
        {
            // Using the SHA-512
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                // Creating an instance of this class already generate a key that can be use as the salt 
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(plainPassword));
            }
        }
    }
}