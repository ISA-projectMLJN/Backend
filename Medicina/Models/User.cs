using System;

namespace Medicina.Models

{
    public enum Role { REGISTER_USER, CAMPAIN_ADMIN, SYSTEM_ADMIN, UNAUTHENTICAN_USER }
    public class User
    {
        public int UserID { get; set; }

        public String Password { get; set; }
        public Role UserRole { get; set; }

        public String Name { get; set; }
        public String Surname { get; set; }



    }
}