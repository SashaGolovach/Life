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
    [Authorize]
    public class AuthController
    {
        private IAuthService _authService;
        private AppSettings _appSettings;

        public AuthController(IAuthService authService, AppSettings appSettings)
        {
            _authService = authService;
            _appSettings = appSettings;
        }
        
        [HttpGet("")]
        public IActionResult AuthGet()
        {
            return new JsonResult("it works");
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]LoginModel loginModel)
        {
            var user = await _authService.Authenticate(loginModel.Username, loginModel.Password);

            if (user == null)
                return new BadRequestResult();
            
            var response = new LoginResponseModel();
            response.Token = CreateToken(user.Id);
            return new JsonResult(response);
        }

        private string CreateToken(string userId)
        {
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.JwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                {
                    new Claim(ClaimTypes.Name, userId)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        [AllowAnonymous]
        [HttpPost("google")]
        public async Task<IActionResult> Google([FromBody]UserView userView)
        {
            try
            {
                var payload = GoogleJsonWebSignature.ValidateAsync(userView.tokenId, new GoogleJsonWebSignature.ValidationSettings()).Result;
                var user = await _authService.Authenticate(payload);
                var response = new LoginResponseModel();
                response.Token = CreateToken(user.Id);
                return new JsonResult(response);
            }
            catch (Exception ex)
            {
                return new BadRequestResult();
            }
        }
    }
}