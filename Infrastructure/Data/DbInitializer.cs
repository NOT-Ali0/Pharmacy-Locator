using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace Infrastructure.Data;

public static class DbInitializer
{
    public static void Seed(AppDbContext context)
    {
        // context.Database.EnsureCreated(); // Moved to Program.cs or verified there.
        // Actually EnsureCreated doesn't run migrations. 
        // Best to use Migrate() in Program.cs, then Seed().
        
        if (context.Medicines.Any())
        {
            return; // DB has been seeded
        }

        var medicines = new List<Medicine>
        {
            new Medicine { Name = "Aspirin", Description = "Pain reliever" },
            new Medicine { Name = "Ibuprofen", Description = "Anti-inflammatory" },
            new Medicine { Name = "Paracetamol", Description = "Fever reducer" },
            new Medicine { Name = "Amoxicillin", Description = "Antibiotic" }
        };

        context.Medicines.AddRange(medicines);
        context.SaveChanges();

        // Seed a demo pharmacy
        var pharmacyUser = new User 
        { 
            Name = "Demo Pharmacy", 
            Email = "pharmacy@demo.com", 
            Role = UserRole.Pharmacy,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password") 
        };
        context.Users.Add(pharmacyUser);
        context.SaveChanges();

        var pharmacy = new Pharmacy
        {
            UserId = pharmacyUser.Id,
            Name = "Downtown Pharmacy",
            Location = new Point(46.6753, 24.7136) { SRID = 4326 }, // Example coords (Riyadh roughly)
            PhoneNumber = "1234567890"
        };
        context.Pharmacies.Add(pharmacy);
        context.SaveChanges();

        // Add stocks
        context.PharmacyMedicines.Add(new PharmacyMedicine
        {
            PharmacyId = pharmacy.Id,
            MedicineId = medicines[0].Id,
            IsAvailable = true,
            Price = 10.5m
        });
        context.SaveChanges();
    }
}
