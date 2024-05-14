using MailKit;
using Medicina.Context;
using Medicina.Models;
using Medicina.String;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Medicina.MailUtil;
using System;
using Medicina.MailUtil;
using Medicina.Service;
using Microsoft.Extensions.Options;

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
        private readonly IOptions<MailSettings> _mailSettingsOptions;
        public PersonController(IOptions<MailSettings> mailSettingsOptions, IConfiguration config, PersonContext personContext, UserContext userContext)
        {
            _config = config;
            _personContext = personContext;
            _userContext = userContext;
            _mailSettingsOptions = mailSettingsOptions;

        }
        [AllowAnonymous]
        [HttpPost("CreatePerson")]
        public IActionResult Create(Person person)
        {
            person.MemberSince = DateTime.Now;
            person.IsActivated = false;
            person.ActivationLink = RandomStringGenerator.RandomString(10);
            _personContext.Add(person);
            _personContext.SaveChanges();
            var user = new User
            {
                Email = person.Email,
                Password = person.Password, // Treba implementirati siguran način čuvanja lozinke
                UserRole = Role.REGISTER_USER,
                Name = person.Name,
                Surname = person.Surname,
                
                CompanyId = 1
            };
            
            _userContext.Add(user);
            _userContext.SaveChanges();

            EmailService emailService = new EmailService(_mailSettingsOptions);
            string body = "http://localhost:4200/ActivateProfile/" + person.ActivationLink;
            emailService.SendEmailAsync(person.Email, "Your activation Link", body);



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

            if (existingPerson == null)
            {
                return NotFound();
            }

            updatedPerson.IsActivated = existingPerson.IsActivated;
            updatedPerson.ActivationLink = existingPerson.ActivationLink;
            _personContext.Entry(existingPerson).CurrentValues.SetValues(updatedPerson);
            existingUser.Password = updatedPerson.Password;
            existingUser.Name = updatedPerson.Name;
            existingUser.Surname = updatedPerson.Surname;

            _userContext.Entry(existingUser).CurrentValues.SetValues(existingUser);
            _personContext.Update(existingPerson);
            _personContext.SaveChanges();
            _userContext.SaveChanges();

            return Ok(existingPerson);
        }

        [HttpGet("GetRegisteredUserById/{id}")]
        public ActionResult<Person> GetUserById(int id)
        {
            var person = _personContext.Persons.Find(id);

            if (person == null)
            {
                return NotFound();
            }

            return Ok(person);
        }

        [HttpPatch("UpdateRegisteredUser")]
        public ActionResult<Person> UpdateRegisteredUser([FromBody] Person updatedPerson)
        {
            var existingPerson = _personContext.Persons.Find(updatedPerson.UserID);
            var existingUser = _userContext.Users.Find(updatedPerson.UserID);

            if (existingPerson == null)
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

        [HttpPut("ActivateProfile/{link}")]
        public ActionResult<Person> ActivateUser([FromRoute] string link)
        {
            var person = _personContext.GetUserWithActivationLink(link);

            if (person == null)
            {
                return NotFound();
            }
            person.IsActivated = true;


            _personContext.Entry(person).CurrentValues.SetValues(person);
            _personContext.SaveChanges();

            return Ok();
        }
       
    }

}


