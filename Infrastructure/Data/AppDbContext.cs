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
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<SupplierMedicine> SupplierMedicines { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

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

        // Supplier - User Relationship
        modelBuilder.Entity<Supplier>()
            .HasOne(s => s.User)
            .WithOne(u => u.Supplier)
            .HasForeignKey<Supplier>(s => s.UserId);

        // SupplierMedicine Configuration
        modelBuilder.Entity<SupplierMedicine>()
            .HasKey(sm => new { sm.SupplierId, sm.MedicineId });

        modelBuilder.Entity<SupplierMedicine>()
            .HasOne(sm => sm.Supplier)
            .WithMany(s => s.SupplierMedicines)
            .HasForeignKey(sm => sm.SupplierId);

        modelBuilder.Entity<SupplierMedicine>()
            .HasOne(sm => sm.Medicine)
            .WithMany()
            .HasForeignKey(sm => sm.MedicineId);

        // Order Configuration
        modelBuilder.Entity<OrderItem>()
            .HasKey(oi => new { oi.OrderId, oi.MedicineId });

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Medicine)
            .WithMany()
            .HasForeignKey(oi => oi.MedicineId);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Pharmacy)
            .WithMany()
            .HasForeignKey(o => o.PharmacyId);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Supplier)
            .WithMany()
            .HasForeignKey(o => o.SupplierId);

        // Seed Data
        modelBuilder.Seed();
    }
}
