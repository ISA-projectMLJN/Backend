using Medicina.Context;
using Medicina.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.AspNetCore.Identity;
using Medicina.String;
using Medicina.MailUtil;

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
        private readonly IMailService _mailService;
      
        public PersonController(IConfiguration config, PersonContext personContext, UserContext userContext, IMailService mailService)
        {
            _config = config;
            _personContext = personContext;
            _userContext = userContext;
            _mailService = mailService;
           
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
                Surname = person.Name,

                CompanyId = 1
            };

            //var result = await _userManager.CreateAsync(user, person.Password);
 
            _userContext.Add(user);
            _userContext.SaveChanges();
            _mailService.SendActivationMail(person);

          
            return Ok("Succes from Create Method");
        }

      /*  private void SendVerificationEmail(string email, string link)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Your App Name", _config["Smtp:Username"]));
            message.To.Add(new MailboxAddress("Recipient Name", email));
            message.Subject = "Verification Email";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = $"Click on the following link to verify your email: {link}";

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.Connect(_config["Smtp:Host"], int.Parse(_config["Smtp:Port"]), false);
                client.Authenticate(_config["Smtp:Username"], _config["Smtp:Password"]);
                client.Send(message);
                client.Disconnect(true);
            }
        } */

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

        [HttpPut("ActivateProfile/{link}")]
        public ActionResult<Person> ActivateUser([FromRoute] string link)
        {
            var person = _personContext.GetUserWithActivationLink(link);
            
            if (person == null )
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