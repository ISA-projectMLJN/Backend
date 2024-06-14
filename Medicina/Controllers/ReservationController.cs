﻿using Medicina.Context;

using Medicina.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using Medicina.MailUtil;

namespace Medicina.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class ReservationController : ControllerBase
    {
        public readonly AppointmentContext _appointmentContext;
        public readonly EquipmentContext _equipmentContext;
        public readonly CompanyContext _companyContext;
        public readonly ReservationContext _reservationContext;
        public readonly UserContext _userContext;
        public readonly PersonContext _personContext;
       // public readonly ComplaintContext _complaintContext;
        public ReservationController(AppointmentContext appointmentContext, EquipmentContext equipmentContext, CompanyContext companyContext, ReservationContext reservationContext, UserContext userContext, PersonContext personContext)
        {
            _appointmentContext = appointmentContext;
            _equipmentContext = equipmentContext;
            _companyContext = companyContext;
            _reservationContext = reservationContext;
            _userContext = userContext;
            _personContext = personContext;
            //_complaintContext = complaintContext;
        }

        [HttpGet("GetAllUncollectedReservations/{companyId}")]
        public ActionResult<IEnumerable<Reservation>> GetAllUncollectedReservations(int companyId)
        {
            var reservations = _reservationContext.Reservations.ToList();
            var appointments = _appointmentContext.Appointments
                .Where(u => u.CompanyId == companyId && u.ReservationId != 0 && u.Status == AppointmentStatus.Reserved).ToList();
            var uncollectedReservations = new List<Reservation>();
            foreach (var res in reservations)
            {
                var app = _appointmentContext.Appointments.FirstOrDefault(u => u.ReservationId == res.Id);
                if (appointments.Contains(app))
                {
                    if (!res.IsCollected)
                    {
                        uncollectedReservations.Add(res);
                    }
                }
            }
            if (uncollectedReservations == null)
            {
                return NotFound();
            }

            return Ok(uncollectedReservations);
        }

        [HttpGet("DidReservationExpire/{reservationId}")]
        public ActionResult<bool> DidReservationExpire(int reservationId)
        {
            var res = _reservationContext.Reservations.Find(reservationId);
            var app = _appointmentContext.Appointments.FirstOrDefault(u => u.ReservationId == res.Id);
            if (res.Deadline <= DateTime.Now)
            {
                var user = _userContext.Users.Find(res.UserId);
                user.PenaltyScore += 2;
                app.ReservationId = 0;
                app.Status = AppointmentStatus.Available;
                _userContext.Update(user);
                _reservationContext.Remove(res);
                _appointmentContext.Update(app);
                _userContext.SaveChanges();
                _reservationContext.SaveChanges();
                _appointmentContext.SaveChanges();
                return Ok(true);
            }
            else return Ok(false);
        }

        [HttpPut("CancelReservation/{reservationId}")]
        public ActionResult CancelReservation(int reservationId)
        {
            var res = _reservationContext.Reservations.Find(reservationId);
            var app = _appointmentContext.Appointments.FirstOrDefault(u => u.ReservationId == res.Id);

            var user = _userContext.Users.Find(res.UserId);

            if (res.Deadline <= DateTime.Now.AddDays(1))
            {
                user.PenaltyScore += 2;
            }
            else
            {
                user.PenaltyScore += 1;
            }
            app.ReservationId = 0;
            app.Status = AppointmentStatus.Available;
            _userContext.Update(user);
            _reservationContext.Remove(res);
            _appointmentContext.Update(app);
            _userContext.SaveChanges();
            _reservationContext.SaveChanges();
            _appointmentContext.SaveChanges();
            return Ok();
        }

        [HttpGet("{id}")]
        public ActionResult<ReservationWithEquipment> GetReservationById(int id)
        {
            var reservation = _reservationContext.Reservations.FirstOrDefault(r => r.Id == id);

            if (reservation == null)
            {
                return NotFound(); // Vrati 404 Not Found ako rezervacija nije pronađena
            }

            // Pronađi opremu na osnovu EquipmentId iz rezervacije
            var equipment = _equipmentContext.Equipment.FirstOrDefault(e => e.Id == reservation.EquipmentId);

            if (equipment == null)
            {
                return NotFound(); // Vrati 404 Not Found ako oprema nije pronađena
            }

            // Kreiraj objekat koji sadrži rezervaciju i pripadajuću opremu
            var reservationWithEquipment = new ReservationWithEquipment
            {
                Reservation = reservation,
                Equipment = equipment
            };
            return reservationWithEquipment;
        }

        [HttpPatch("ReservationCollected/{reservationId}")]
        public ActionResult<bool> ReservationCollected(int reservationId)
        {
            var res = _reservationContext.Reservations.Find(reservationId);
            var app = _appointmentContext.Appointments.FirstOrDefault(u => u.ReservationId == res.Id);
            app.Status = AppointmentStatus.Collected;
            res.IsCollected = true;
            var eq = _equipmentContext.Equipment.FirstOrDefault(e => e.Id == res.EquipmentId);
            eq.Count -= res.EquipmentCount;
            if (eq.Count < 0)
            {
                return Ok(false);
            } 
            _appointmentContext.Update(app);
            _reservationContext.Update(res);
            _equipmentContext.Update(eq);
            _reservationContext.SaveChanges();
            _appointmentContext.SaveChanges();
            _equipmentContext.SaveChanges();

            // Pronađi osobu na osnovu korisničkog ID-a
            var person = _personContext.Persons.FirstOrDefault(p => p.UserID == res.UserId);

            // Pozovi metodu za slanje potvrde mejla ako je pronađena osoba


            return Ok(true);
        }
  
        [HttpGet("GetAllFutureReservations")]
        public ActionResult<IEnumerable<Reservation>> GetAllFutureReservations()
        {
            var futureReservations = _reservationContext.Reservations
                .Where(r => r.Deadline > DateTime.Now)
                .ToList();

            if (futureReservations == null || futureReservations.Count == 0)
            {
                return NotFound();
            }

            return Ok(futureReservations);
        }

        [HttpGet("GetReservationsForCompany/{companyId}")]
        public ActionResult<IEnumerable<Reservation>> GetReservationsForCompany(int companyId)
        {
            var reservations = _reservationContext.Reservations
                .Where(r => r.CompanyId == companyId)
                .ToList();

            if (reservations == null || reservations.Count == 0)
            {
                return NotFound("No reservations found for the specified company.");
            }

            return Ok(reservations);
        }
       


    }





    // Klasa koja predstavlja rezervaciju sa pripadajućom opremom
    public class ReservationWithEquipment
        {
            public Reservation Reservation { get; set; }
            public Equipment Equipment { get; set; }
        }
    }

