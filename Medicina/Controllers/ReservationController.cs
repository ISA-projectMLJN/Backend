using Medicina.Context;
using Medicina.Migrations.Reservation;
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
    public class ReservationController : ControllerBase
    {
        public readonly AppointmentContext _appointmentContext;
        public readonly EquipmentContext _equipmentContext;
        public readonly CompanyContext _companyContext;
        public readonly ReservationContext _reservationContext;
        public readonly UserContext _userContext;
        public ReservationController(AppointmentContext appointmentContext, EquipmentContext equipmentContext, CompanyContext companyContext, ReservationContext reservationContext, UserContext userContext)
        {
            _appointmentContext = appointmentContext;
            _equipmentContext = equipmentContext;
            _companyContext = companyContext;
            _reservationContext = reservationContext;
            _userContext = userContext;
        }

        [HttpGet("GetAllUncollectedReservations/{companyId}")]
        public ActionResult<IEnumerable<Reservation>> GetAllUncollectedReservations(int companyId)
        {
            var reservations = _reservationContext.Reservations.ToList();
            var appointments = _appointmentContext.Appointments.ToList()
                .Where(u => u.CompanyId == companyId && u.ReservationId != 0 && u.Status == AppointmentStatus.Reserved);
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


        [HttpPatch("ReservationCollected/{reservationId}")]
        public ActionResult<bool> ReservationCollected(int reservationId)
        {
            var res = _reservationContext.Reservations.Find(reservationId);
            var app = _appointmentContext.Appointments.FirstOrDefault(u => u.ReservationId == res.Id);
                app.Status = AppointmentStatus.Collected;
                res.IsCollected = true;
                var eq = _equipmentContext.Equipment.FirstOrDefault(e=> e.Id == res.EquipmentId);
                eq.Count -= res.EquipmentCount;
                _appointmentContext.Update(app);
                _reservationContext.Update(res);
                _equipmentContext.Update(eq);
                _reservationContext.SaveChanges();
                _appointmentContext.SaveChanges();
                _equipmentContext.SaveChanges();
                return Ok(true);
        }
    }
}