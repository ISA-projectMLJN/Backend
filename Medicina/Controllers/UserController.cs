using Medicina.Context;
using Medicina.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace Medicina.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _config;
        public readonly UserContext _userContext;
        public UserController(IConfiguration config, UserContext userContext)
        {
            _config = config;
            _userContext = userContext;
        }
        [AllowAnonymous]
        [HttpPost("CreateUser")]
        public IActionResult Create(User user)
        {
            if (_userContext.Users.Where(u => u.Email == user.Email).FirstOrDefault() != null)
            {
                return Ok("Already Exists");
            }

            _userContext.Add(user);
            _userContext.SaveChanges();
            return Ok("Succes from Create Method");
        }
        [AllowAnonymous]
        [HttpPost("LoginUser")]
        public IActionResult Login(LogIn user)
        {
            var userAvailable = _userContext.Users.Where(u => u.Email == user.Email && u.Password == user.Password).FirstOrDefault();
            if (userAvailable != null)
             {
                return Ok(new JwtService(_config).GenerateToken(
                    userAvailable.UserID.ToString(),
                    userAvailable.Email,
                    userAvailable.Password,
                    userAvailable.Name,
                    userAvailable.Surname
                    
                    ));
             }
            else
            {
                return Ok("Fail");
            }

        }
    }
        
            
    
}







     
