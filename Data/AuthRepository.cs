using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace battleship_server.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;

        public AuthRepository(DataContext dataContext, IConfiguration configuration)
        {
            _dataContext = dataContext;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<string>> Login(string username, string password)
        {
            var servicesResponse = new ServiceResponse<string>();

            // Get the user from the DB
            var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Username.ToLower().Equals(username.ToLower()));

            // If no matching username found
            if (user is null)
            {
                servicesResponse.Success = false;
                servicesResponse.Message = "Username does not exist!";
            }
            // Username found
            else
            {
                if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                {
                    servicesResponse.Success = false;
                    servicesResponse.Message = "Incorrect password!";
                }
                // Correct
                else
                {
                    // Create and send back the JWT
                    servicesResponse.Data = CreateJWTToken(user);
                }
            }

            // Return the response
            return servicesResponse;
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            var response = new ServiceResponse<int>();

            // Check if user already existed
            if (await UserExists(user.Username))
            {
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

        // Method for checking the password match
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        // Method for creating JWT
        private string CreateJWTToken(User user)
        {
            var claims = new List<Claim>{
                // The list of Claims for the JWT
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            // Get the Secret token from AppSettings.json
            var appSettingsToken = _configuration.GetSection("AppSettings:Token").Value;

            // Check if appSettings is null
            if (appSettingsToken is null)
            {
                throw new Exception("AppSettings token is null");
            }

            // Using the token
            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(appSettingsToken));

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // Token description
            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            // The JWT handler
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}