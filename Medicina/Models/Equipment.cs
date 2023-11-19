using System;
using System.Collections.Generic;

namespace Medicina.Models
{
    public class Equipment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public double Rating { get; set; }
        public List<EquipmentCompany> EquipmentCompanies { get; set; }

        public Equipment()
        {
        }

        // Konstruktor koji postavlja osnovne informacije
        public Equipment(string name, string type, string description, double rating)
        {
            Name = name;
            Type = type;
            Description = description;
            Rating = rating;
        }

        // Konstruktor koji prima sve informacije, uključujući i listu EquipmentCompany
        public Equipment(string name, string type, string description, double rating, List<EquipmentCompany> equipmentCompanies)
        {
            Name = name;
            Type = type;
            Description = description;
            Rating = rating;
            EquipmentCompanies = equipmentCompanies;
        }
    }
}

