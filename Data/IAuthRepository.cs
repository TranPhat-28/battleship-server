using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace battleship_server.Data
{
    public interface IAuthRepository
    {
        // Register returns the ID of the newly created user
        Task<ServiceResponse<int>> Register(User user, string password);
        // Login returns a token
        Task<ServiceResponse<string>> Login(string username, string password);
        // UserExists return a bool
        Task<bool> UserExists(string username);
    }
}