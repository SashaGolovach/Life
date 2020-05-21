using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth;
using Life.Helpers;
using Life.Models;
using Life.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Life.Controllers
{
    [Route("auth")]
    public class AuthController
    {
        private IAuthService _authService;
        private UsersService _usersService;

        public AuthController(IAuthService authService, UsersService usersService)
        {
            _authService = authService;
            _usersService = usersService;
        }
        
        [HttpPost("user")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser([FromBody]User user)
        {
            _usersService.Create(user);
            return new OkResult();
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            var user = _authService.Authenticate(loginModel.Username, loginModel.Password);

            if (user == null)
                return new BadRequestResult();

            return new JsonResult(user);
        }

        [AllowAnonymous]
        [HttpPost("google")]
        public async Task<IActionResult> Google([FromBody]UserView userView)
        {
            try
            {
                var payload = GoogleJsonWebSignature.ValidateAsync(userView.tokenId, new GoogleJsonWebSignature.ValidationSettings()).Result;
                var user = await _authService.Authenticate(payload);

                // var claims = new[]
                // {
                //     new Claim(JwtRegisteredClaimNames.Sub, Security.Encrypt(AppSettings.appSettings.JwtEmailEncryption,user.email)),
                //     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                // };

                var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AppSettings.appSettings.JwtSecret));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(String.Empty,
                    String.Empty,
                    new List<Claim>(),
                    expires: DateTime.Now.AddSeconds(55*60),
                    signingCredentials: creds);
                return new OkResult();
            }
            catch (Exception ex)
            {
                return new BadRequestResult();
            }
        }
    }
}