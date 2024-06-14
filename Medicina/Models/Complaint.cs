using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medicina.Models
{
    public enum Status { SENT, ANSWERED }
    public class Complaint
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int? AdministratorId { get; set; }
        public string ComplaintText { get; set; }
        public int UserId { get; set; }
        public string Answer {  get; set; }
        public Status StatusComplaint { get; set; }
        public Complaint() { }

        public Complaint(int id, int companyId, int administratorId, string complaintText, int userId)
        {
            Id = id;
            CompanyId = companyId;
            AdministratorId = administratorId;
            ComplaintText = complaintText;
            UserId = userId;
        }
    }
}
