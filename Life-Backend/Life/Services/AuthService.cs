using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Life.Models;

namespace Life.Services
{
    public class AuthService : IAuthService
    {
        private UsersService _usersService;
        
        public AuthService(UsersService usersService)
        {
            _usersService = usersService;
        }

        public async Task<User> Authenticate(Google.Apis.Auth.GoogleJsonWebSignature.Payload payload)
        {            
            var user = _usersService.Get(payload.Email);
            return user;
        }
        
        public async Task<User> Authenticate(string username, string password)
        {
            var user = _usersService.Get(username);
            if (user?.Password == password)
                return user;
            return null;
        }
    }
}