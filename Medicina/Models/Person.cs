﻿using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace Medicina.Models
{
    public class Person
    {
        [Key]
        public int UserID { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
        public String Name { get; set; }
        public String Surname { get; set; }
        public String City { get; set; }
        public String Country { get; set; }
        public String Phone { get; set; }
        public String Profession { get; set; }
        public String CompanyInfo { get; set; }
        public DateTime MemberSince { get; set; }
    }
}
