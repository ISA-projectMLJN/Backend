using Medicina.Context;
using Medicina.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [AllowAnonymous]
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
        [HttpGet("GetAdminById/{id}")]
        public ActionResult<Person> GetById(int id)
        {
            var person = _personContext.Persons.Find(id);

            if (person == null)
            {
                return NotFound();
            }

            return Ok(person);
        }
        [HttpPatch("UpdateAdmin")]
        public ActionResult<Person> UpdateAdmin([FromBody] Person updatedPerson)
        {
            var existingPerson = _personContext.Persons.Find(updatedPerson.UserID);
            var existingUser = _userContext.Users.Find(updatedPerson.UserID);

            if (existingPerson == null || existingUser == null)
            {
                return NotFound();
            }

            _personContext.Entry(existingPerson).CurrentValues.SetValues(updatedPerson);
            existingUser.Password = updatedPerson.Password;
            existingUser.UserRole = Role.SYSTEM_ADMIN;
            existingUser.Name = updatedPerson.Name;
            existingUser.Surname = updatedPerson.Surname;

            _userContext.Entry(existingUser).CurrentValues.SetValues(existingUser);
            _personContext.SaveChanges();
            _userContext.SaveChanges();

            return Ok(existingPerson);
        }

        [HttpGet("GetUserById/{id}")]
        public ActionResult<Person> GetUserById(int id)
        {
            var person = _personContext.Persons.Find(id);

            if (person == null)
            {
                return NotFound();
            }

            return Ok(person);
        }

        [HttpPatch("UpdateUser")]
        public ActionResult<Person> UpdateUser([FromBody] Person updatedPerson)
        {
            var existingPerson = _personContext.Persons.Find(updatedPerson.UserID);
            var existingUser = _userContext.Users.Find(updatedPerson.UserID);

            if (existingPerson == null || existingUser == null)
            {
                return NotFound();
            }

            _personContext.Entry(existingPerson).CurrentValues.SetValues(updatedPerson);
            existingUser.Password = updatedPerson.Password;
            existingUser.UserRole = Role.REGISTER_USER;
            existingUser.Name = updatedPerson.Name;
            existingUser.Surname = updatedPerson.Surname;

            _userContext.Entry(existingUser).CurrentValues.SetValues(existingUser);
            _personContext.SaveChanges();
            _userContext.SaveChanges();

            return Ok(existingPerson);
        }
    }
}