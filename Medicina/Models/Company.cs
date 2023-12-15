using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medicina.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public double AverageRating { get; set; }
        public string AvailablePickupDates { get; set; } 
        public List<User> OtherAdministrators { get; set; }
        [NotMapped]
        public List<Equipment> CompaniesEquipment { get; set; }
        [NotMapped]
        public Equipment? Equipment { get; set; }
        public int? EquipmentId { get; set; }
        public Company()
        {
        }

        public Company(string name, string address, string description, double averageRating)
        {
            Name = name;
            Address = address;
            Description = description;
            AverageRating = averageRating;
        }


        public Company(string name, string address, string description, double averageRating,
            string availablePickupDates, List<User> otherAdministrators,
            List<Equipment> equipmentCompanies)
        {
            Name = name;
            Address = address;
            Description = description;
            AverageRating = averageRating;
            AvailablePickupDates = availablePickupDates;
            OtherAdministrators = otherAdministrators;
            CompaniesEquipment = equipmentCompanies;
        }

    }
}