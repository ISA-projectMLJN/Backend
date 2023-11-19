using Medicina.Models;
using Microsoft.EntityFrameworkCore;

public class EquipmentCompanyContext : DbContext
{
    public DbSet<EquipmentCompany> EquipmentCompanies { get; set; }

    public EquipmentCompanyContext(DbContextOptions<EquipmentCompanyContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EquipmentCompany>()
            .HasKey(ec => new { ec.EquipmentId, ec.CompanyId });

        // Dodatna konfiguracija ako je potrebna.

        base.OnModelCreating(modelBuilder);
    }
}