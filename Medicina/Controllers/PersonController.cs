using Medicina.Context;
using Medicina.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace Medicina.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class PersonController : ControllerBase
    {
        private readonly IConfiguration _config;
        public readonly PersonContext _personContext;
        public readonly UserContext _userContext;
        public PersonController(IConfiguration config, PersonContext personContext, UserContext userContext)
        {
            _config = config;
            _personContext = personContext;
            _userContext = userContext; 
        }

        [HttpPost("CreatePerson")]
        public IActionResult Create(Person person)
        {
            person.MemberSince = DateTime.Now;
            _personContext.Add(person);
            _personContext.SaveChanges();
            var user = new User
            {
                Password = person.Password, // Treba implementirati siguran način čuvanja lozinke
                UserRole = Role.REGISTER_USER,
                Name = person.Name,
                Surname = person.Surname
            };

            _userContext.Add(user);
            _userContext.SaveChanges();

            return Ok("Succes from Create Method");
        }

    }
}


//https://localhost:44356/
