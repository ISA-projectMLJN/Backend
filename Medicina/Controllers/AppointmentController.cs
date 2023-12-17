using Medicina.Context;
using Medicina.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
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
        public readonly ReservationContext _reservationContext;
        public readonly UserContext _userContext;
        public readonly PersonContext _presonContext;
        public AppointmentController(AppointmentContext appointmentContext, EquipmentContext equipmentContext, CompanyContext companyContext, ReservationContext reservationContext, UserContext userContext, PersonContext personContext)
        {
            _appointmentContext = appointmentContext;
            _equipmentContext = equipmentContext;
            _companyContext = companyContext;
            _reservationContext = reservationContext;
            _userContext = userContext; 
            _presonContext = personContext;
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

            var admin = _presonContext.Persons.Find(newAppointment.AdministratorsId);
            newAppointment.AdministratorsName = admin.Name;
            newAppointment.AdministratorsSurname = admin.Surname;

            _appointmentContext.Appointments.Add(newAppointment);
            _appointmentContext.SaveChanges();

            return CreatedAtAction(nameof(GetAppointmentsByCompanyId), new { id = newAppointment.CompanyId }, newAppointment);
        }
        [HttpPatch("ReserveAppointment/{id}")]
        public IActionResult ReserveAppointment(int id, [FromBody] Reservation reservation)
        {
            Console.WriteLine($"Received id: {id}, userId: {reservation.UserId}");

            var appointment = _appointmentContext.Appointments.Find(id);
            if (appointment == null)
            {
                return NotFound();
            }

            // Assuming there's a property like UserId in your Appointment model
            //appointment.UserId = userId;

            // Set the appointment as reserved
            //dodavanje rezervacije 
            
            _reservationContext.Reservations.Add(reservation);
            _reservationContext.SaveChanges();

            appointment.IsReserved = true;
            appointment.ReservationId = reservation.Id;
            _appointmentContext.Entry(appointment).State = EntityState.Modified;
            _appointmentContext.SaveChanges();

            return Ok(appointment);
        }
        [HttpGet("GetAppointmentsForDay")]
        public ActionResult<IEnumerable<object>> GetAppointmentsForDay([FromQuery] DateTime date)
        {
            var appointmentsForDay = _appointmentContext.Appointments .ToList();

            return Ok(appointmentsForDay);
        }

       


    }
}