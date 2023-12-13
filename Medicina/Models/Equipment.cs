using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medicina.Models
{
    public class Equipment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public double Rating { get; set; }
        [NotMapped]
        public List<Company> EquipmentsCompanies { get; set; }
        [NotMapped]
        public Company? Company { get; set; }
        [NotMapped]
        public int? CompanyId { get; set; }

        public Equipment()
        {
        }

        public Equipment(string name, string type, string description, double rating)
        {
            Name = name;
            Type = type;
            Description = description;
            Rating = rating;
        }

        public Equipment(string name, string type, string description, double rating, List<Company> equipmentCompanies)
        {
            Name = name;
            Type = type;
            Description = description;
            Rating = rating;
            EquipmentsCompanies = equipmentCompanies;
        }
    }
}
