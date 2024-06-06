namespace Medicina.Models
{
    public class Complaint
    {
        private int Id { get; set; }
        private int CompanyId { get; set; }
        private int? AdministratorId { get; set; }
        private string ComplaintText { get; set; }
        public int UserId { get; set; }
    }
}
