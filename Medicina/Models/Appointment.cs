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
        public int? UserId { get; set; }
        [NotMapped]
        public string? AdministratorsName { get; set; }
        [NotMapped]
        public string? AdministratorsSurname { get; set; }
        public DateTime Date { get; set; }
        public int Duration { get; set; }
        public bool IsReserved { get; set; }
        public Appointment() { }
        public Appointment(int administratorsId, int companyId, int userId, DateTime date, int duration, bool isReserved)
        {
            AdministratorsId = administratorsId;
            CompanyId = companyId;
            UserId = userId;
            Date = date;
            Duration = duration;
            IsReserved = isReserved;
        }
    }
}
