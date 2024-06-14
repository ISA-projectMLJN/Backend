using Medicina.Context;
using Medicina.DTO;
using Medicina.MailUtil;
using Medicina.Models;
using Medicina.Service;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;

namespace Medicina.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class ComplaintController : ControllerBase
    {
        private readonly ComplaintContext _complaintContext;
        private readonly UserContext _userContext;
        private readonly IOptions<MailSettings> _mailSettingsOptions;


        public ComplaintController(ComplaintContext cContext, UserContext uContext, IOptions<MailSettings> mailSettingsOptions)
        {
            _complaintContext = cContext;
            _userContext = uContext;
            _mailSettingsOptions = mailSettingsOptions;
        }

        [HttpPost("SubmitComplaint")]
        public async Task<IActionResult> SubmitComplaint(Complaint complaint)
        {
            if (complaint == null)
            {
                return BadRequest("Invalid complaint data.");
            }
            Console.WriteLine($"Received complaint: {complaint.ComplaintText}");

            // Dodavanje žalbe u kontekst

            complaint.StatusComplaint = Status.SENT;
            _complaintContext.Complaints.Add(complaint);
            await _complaintContext.SaveChangesAsync();




            return Ok("Complaint submitted successfully.");
        }
        [HttpGet("GetAllComplaints")]
        public ActionResult<IEnumerable<Complaint>> GetAllComplaints()
        {
            var complaints = _complaintContext.Complaints.ToList();

            if (complaints == null)
            {
                return NotFound();
            }

            return Ok(complaints);
        }

        [HttpPut("AnswerOnComplaintId")]
        public ActionResult AnswerOnComplaintId([FromBody] ComplaintDTO complaintDTO)
        {
            if (string.IsNullOrEmpty(complaintDTO.Text))
            {
                return BadRequest("Answer text cannot be empty.");
            }

            var complaint = _complaintContext.Complaints.FirstOrDefault(c => c.Id == complaintDTO.Id);
            if (complaint == null)
            {
                return NotFound();
            }

            complaint.Answer = complaintDTO.Text;
            complaint.StatusComplaint = Status.ANSWERED;
            _complaintContext.SaveChanges();

            var user = _userContext.Users.FirstOrDefault(u => u.UserID == complaint.UserId);
            if (user != null)
            {
                EmailService emailService = new EmailService(_mailSettingsOptions);
                var subject = "Complaint Answered";
                var body = $"<p>Your complaint has been answered:</p><p>{complaintDTO.Text}</p>";
                emailService.SendEmailAnswerAsync(user.Email, subject, body);
            }

            return Ok();
        }

    }
}




