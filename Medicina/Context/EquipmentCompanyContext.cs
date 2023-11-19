using Medicina.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

public class EquipmentCompanyContext : DbContext
{
    public DbSet<EquipmentCompany> EquipmentCompany { get; set; }

    public EquipmentCompanyContext(DbContextOptions<EquipmentCompanyContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EquipmentCompany>()
            .HasKey(ec => new { ec.EquipmentId, ec.CompanyId });


        base.OnModelCreating(modelBuilder);

    }


}
