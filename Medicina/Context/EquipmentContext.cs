using Medicina.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Medicina.Context
{
    public class EquipmentContext : DbContext
    {
        public EquipmentContext(DbContextOptions<EquipmentContext> options) : base(options)
        {

        }

        public DbSet<Equipment> Equipment { get; set; }

      

        public List<Equipment> GetAll()
        {
            var equipments = Equipment
                .Include(e => e.EquipmentCompanies)
                .ToList();

            return equipments;
        }



    }
}
