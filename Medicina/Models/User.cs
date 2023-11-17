using System;

namespace Medicina.Models
{
    public class User
    {
        public int UserID { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
        public String Name { get; set; }
        public String Surname { get; set; }
        public String CIty { get; set; }
        public String Country { get; set; }
        public String Phone { get; set; }
        public String Profession { get; set; }
        public String CompanyInfo { get; set; }
        public DateTime MemberSince { get; set; }


    }
}
