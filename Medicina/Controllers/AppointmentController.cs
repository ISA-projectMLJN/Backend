﻿using Medicina.Context;
using Medicina.MailUtil;
using Medicina.Models;
using Medicina.Service;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
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

        private readonly IOptions<MailSettings> _mailSettingsOptions;

        public AppointmentController(IOptions<MailSettings> mailSettingsOptions, AppointmentContext appointmentContext, EquipmentContext equipmentContext, CompanyContext companyContext, ReservationContext reservationContext, UserContext userContext, PersonContext personContext)
        {
            _appointmentContext = appointmentContext;
            _equipmentContext = equipmentContext;
            _companyContext = companyContext;
            _reservationContext = reservationContext;
            _userContext = userContext; 
            _presonContext = personContext;
            _mailSettingsOptions = mailSettingsOptions;

        }
        [HttpGet("GetAppointmentsByCompanyId/{id}")]
        public ActionResult<Appointment> GetAppointmentsByCompanyId(int id)
        {
            var AppointmentList = _appointmentContext.Appointments.Where(e => e.CompanyId == id && e.Status == AppointmentStatus.Available).ToList();

            if (AppointmentList == null)
            {
                return NotFound();
            }
            return Ok(AppointmentList);
        }

        [HttpGet("GetAppointmentsByDateAndWorking/{id}/{date}")]
        public ActionResult<Appointment> GetAppointmentsByDateAndWorking(int id, DateTime date)
        {
            // Filtriraj termine za određeni datum
            var appointmentList = _appointmentContext.Appointments
                .Where(e => e.CompanyId == id && e.Start.Date == date.Date)
                .ToList();

            if (appointmentList == null)
            {
                return NotFound();
            }

            return Ok(appointmentList);
        }



        [HttpPost("AddAppointment")]
        public ActionResult<Appointment> AddAppointment([FromBody] Appointment newAppointment)
        {
            if (newAppointment == null)
            {
                return BadRequest();
            }

            var admin = _presonContext.Persons.Find(newAppointment.AdministratorsId);
            var company = _companyContext.Companies.FirstOrDefault(e => e.Id == newAppointment.CompanyId);
            newAppointment.AdministratorsName = admin.Name;
            newAppointment.AdministratorsSurname = admin.Surname;

            newAppointment.Status = AppointmentStatus.Available;
            newAppointment.EndTime = newAppointment.Start.AddMinutes(newAppointment.Duration);
            TimeSpan startTimeOfDay = new TimeSpan(newAppointment.Start.TimeOfDay.Hours, newAppointment.Start.TimeOfDay.Minutes, newAppointment.Start.TimeOfDay.Seconds);
            TimeSpan endTimeOfDay = new TimeSpan(newAppointment.EndTime.TimeOfDay.Hours, newAppointment.EndTime.TimeOfDay.Minutes, newAppointment.EndTime.TimeOfDay.Seconds);
            
            if (startTimeOfDay < company.OpeningTime || endTimeOfDay > company.ClosingTime)
            {
                return BadRequest("Appointment is outside of working hours.");
            }

            var adminsAppointments = _appointmentContext.Appointments
                .Where(e => e.AdministratorsId == admin.UserID)
                .ToList();
            var allAppointments = _appointmentContext.Appointments.ToList();

            foreach (var app in allAppointments)
            {
                if ((app.Start <= newAppointment.Start && app.EndTime >= newAppointment.EndTime) ||
                    (app.Start >= newAppointment.Start && app.EndTime <= newAppointment.EndTime) ||
                    (app.Start < newAppointment.EndTime && app.EndTime > newAppointment.Start))
                {
                    return BadRequest("Appointment conflicts with another appointment.");
                }
            }


            _appointmentContext.Appointments.Add(newAppointment);
            _appointmentContext.SaveChanges();
            _companyContext.SaveChanges();

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
            

            reservation.Deadline = appointment.EndTime;
            reservation.Name = "Lidija";
            reservation.Surname = "Djurdjic";
            _reservationContext.Reservations.Add(reservation);
            _reservationContext.SaveChanges();

            appointment.Status = AppointmentStatus.Reserved;
            appointment.ReservationId = reservation.Id;
          
            _appointmentContext.Entry(appointment).State = EntityState.Modified;
            _appointmentContext.SaveChanges();


            EmailService emailService = new EmailService(_mailSettingsOptions);
            string body = "You have succesfuly reserved appointment. Here is your qr code";

            
            emailService.SendEmailWithQrCodeAsync("lidija.dj17@gmail.com", "Your qr code", body, reservation);
            
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