using Delivery.API.Repository.Account;
using Delivery.API.Services;
using Delivery.Models;
using Delivery.Models.ModelView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Delivery.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository repo;
        private readonly IConfiguration _config;

        public AccountController(IAccountRepository repository, IConfiguration config)
        {
            repo = repository;
            _config = config;
        }

        // Register
        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel register)
        {
            if (ModelState.IsValid)
            {
                var user = await repo.RegisterAsync(register);
                if (user != null)
                    return Ok();
            }
            if (AppServices.ErrorMessage != null)
                return BadRequest(AppServices.ErrorMessage);

            return BadRequest();
        }

        // Login
        [Route("Login")]
        [HttpPost]
        public async Task<ActionResult<TokenModel>> Login(LoginModel login)
        {
            if (ModelState.IsValid)
            {
                var user = await repo.LoginAsync(login);
                if (user != null)
                {
                    await repo.IsInRoleAsync(user);
                    var roleName = await repo.GetRoleNameByUserId(user.Id);
                    if (roleName == null || string.IsNullOrEmpty(roleName))
                        return BadRequest();
                    var model = new TokenModel
                    {
                        Email = user.Email,
                        UserName = user.UserName,
                        Token = await GenerateToken(user, roleName),
                        Role = roleName
                    };
                    return model;
                }
            }
            if (AppServices.ErrorMessage != null)
                return BadRequest(AppServices.ErrorMessage);

            return BadRequest();
        }

        private async Task<string> GenerateToken(ApplicationUser user, string roleName)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.GetSection("AppSettings:SecretKey").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, roleName)
                }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Expires = DateTime.UtcNow.AddYears(1),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var strTokn = tokenHandler.WriteToken(token);
            return await Task.FromResult(strTokn);
        }

        [Authorize(Roles = "User")]
        [Route("Test")]
        [HttpGet]
        public IActionResult Test()
        {
            var name = User.FindFirst(ClaimTypes.Name)?.Value;
            return Ok();
        }


    }
}
