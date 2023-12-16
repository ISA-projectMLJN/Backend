using Medicina.Context;
using Medicina.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace Medicina.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class AppointmentController : ControllerBase
    {
        public readonly AppointmentContext _appointmentContext;
        public readonly EquipmentContext _equipmentContext;
        public readonly CompanyContext _companyContext;
        public AppointmentController(AppointmentContext appointmentContext, EquipmentContext equipmentContext, CompanyContext companyContext)
        {
            _appointmentContext = appointmentContext;
            _equipmentContext = equipmentContext;
            _companyContext = companyContext;
        }
        [HttpGet("GetAppointmentsByCompanyId/{id}")]
        public ActionResult<Appointment> GetAppointmentsByCompanyId(int id)
        {
            var AppointmentList = _appointmentContext.Appointments.Where(e => e.CompanyId == id).ToList();

            if (AppointmentList == null)
            {
                return NotFound();
            }
            return Ok(AppointmentList);
        }
        [HttpPost("AddAppointment")]
        public ActionResult<Appointment> AddAppointment([FromBody] Appointment newAppointment)
        {
            if (newAppointment == null)
            {
                return BadRequest();
            } 

            _appointmentContext.Appointments.Add(newAppointment);
            _appointmentContext.SaveChanges();

            return CreatedAtAction(nameof(GetAppointmentsByCompanyId), new { id = newAppointment.CompanyId }, newAppointment);
        }
    }
}