using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Pharmacy> Pharmacies { get; set; }
    public DbSet<Medicine> Medicines { get; set; }
    public DbSet<PharmacyMedicine> PharmacyMedicines { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Enable PostGIS extension
        modelBuilder.HasPostgresExtension("postgis");

        // PharmacyMedicine Relationships
        modelBuilder.Entity<PharmacyMedicine>()
            .HasOne(pm => pm.Pharmacy)
            .WithMany(p => p.PharmacyMedicines)
            .HasForeignKey(pm => pm.PharmacyId);

        modelBuilder.Entity<PharmacyMedicine>()
            .HasOne(pm => pm.Medicine)
            .WithMany(m => m.PharmacyMedicines)
            .HasForeignKey(pm => pm.MedicineId);

        // Pharmacy - User Relationship
        modelBuilder.Entity<Pharmacy>()
            .HasOne(p => p.User)
            .WithOne(u => u.Pharmacy)
            .HasForeignKey<Pharmacy>(p => p.UserId);
    }
}
