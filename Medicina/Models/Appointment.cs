using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Xml.Linq;

namespace Medicina.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int AdministratorsId { get; set; }
        public int CompanyId { get; set; }
        public int? ReservationId { get; set; }
        public int EquipmentId { get; set; }
        
        public string? AdministratorsName { get; set; }
        
        public string? AdministratorsSurname { get; set; }
        public DateTime Date { get; set; }
        public int Duration { get; set; }
        public bool IsReserved { get; set; }
        public bool IsEquipmentTaken { get; set; }

        public Appointment() { }
        public Appointment(int administratorsId, int companyId, int resId, DateTime date, int duration, bool isReserved, bool isTaken)
        {
            AdministratorsId = administratorsId;
            CompanyId = companyId;
            ReservationId = resId; 
            Date = date;
            Duration = duration;
            IsReserved = isReserved;
            IsEquipmentTaken = isTaken;
        }
    }
}
