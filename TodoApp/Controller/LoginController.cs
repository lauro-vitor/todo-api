using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TodoApp.DAL;
using TodoApp.Models;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TodoApp.Controller
{
    [ApiController]
    [Route("/api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly IConfiguration _configuration;

        public LoginController(TodoContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
       
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            if (ModelState.IsValid)
            {
                User user = AuthenticateUser(login);
                if(user != null)
                {
                    string token = GenerateJsonWebToken(user);
                    return Ok(new {
                         user,
                        Token = token
                    });
                }
            }

            return Unauthorized();
        }
        public User AuthenticateUser(LoginModel loginModel)
        {
            User user = _context.User.FirstOrDefault(u => 
                    u.Login.ToLower().Trim().Equals(loginModel.Login.ToLower().Trim()) && 
                    u.Password.ToLower().Trim().Equals(loginModel.Password.ToLower().Trim()));
            
            return user;
        }
        public string GenerateJsonWebToken(User user)
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecreetKey"]));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            Claim[] claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Name)
            };

            JwtSecurityToken token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"], 
                claims, 
                expires: DateTime.Now.AddHours(2), 
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        
    }
}
