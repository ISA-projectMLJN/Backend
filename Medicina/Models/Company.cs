﻿using System;
using System.Collections.Generic;

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
        public List<EquipmentCompany> EquipmentCompanies { get; set; }

        // Prazan konstruktor
        public Company()
        {
        }

        // Konstruktor koji postavlja osnovne informacije
        public Company(string name, string address, string description, double averageRating)
        {
            Name = name;
            Address = address;
            Description = description;
            AverageRating = averageRating;
        }

        // Konstruktor koji prima sve informacije
        public Company(string name, string address, string description, double averageRating,
            string availablePickupDates, List<User> otherAdministrators,
            List<EquipmentCompany> equipmentCompanies)
        {
            Name = name;
            Address = address;
            Description = description;
            AverageRating = averageRating;
            AvailablePickupDates = availablePickupDates;
            OtherAdministrators = otherAdministrators;
            EquipmentCompanies = equipmentCompanies;
        }
    }
}