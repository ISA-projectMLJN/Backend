using Medicina.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using Medicina.DTO;
using Medicina.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Cors;

namespace Medicina.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowOrigin")]
    public class AuthController : ControllerBase
    {
        //private readonly IUserService userService;
        private readonly IConfiguration _config;
        public readonly PersonContext _personContext;

        public AuthController(PersonContext personContext, IConfiguration projectConfig)
        {
            _personContext = personContext;
            _config = projectConfig;

        }





        [Route("login")]
        [HttpPost]
        public IActionResult Login(LoginDTO loginDTO)
        {


            if (loginDTO == null || loginDTO.Email == null || loginDTO.Password == null)
            {
                return BadRequest("Invalid client request");
            }

            Person person = _personContext.GetUserWithEmailAndPassword(loginDTO.Email, loginDTO.Password);
            if (person == null || loginDTO.Password != person.Password)
            {
                return BadRequest("Invalid credentials!");
            }
            if (!person.IsActivated)
            {
                return BadRequest("Account not activated!");
            }
            //if (person == null || !BCrypt.Net.BCrypt.Verify(loginDTO.Password, person.Password))
            //{
            //  return BadRequest("Invalid credentials!");
            //}

            Claim[] claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "hkz2Ba9cf2Q4lPjAf6mS"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("Id",person.UserID.ToString()),
                new Claim("Email",person.Email)
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("zxzxzxzxzxrltCPJ9e6jzxczckCq5nrPP5A"));
            SigningCredentials signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken token = new JwtSecurityToken("bOH8NLMXtivXMrB6c9ED", "wEoprCagCl0G5ySSfZxA", claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);
            string accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            TokenDTO tokenDTO = new TokenDTO() { Token = accessToken };
            return Ok(tokenDTO);
        }
    }
}