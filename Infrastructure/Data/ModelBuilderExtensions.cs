using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public static class ModelBuilderExtensions
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        // --- 1. Seed Medicines ---
        var medicine1Id = Guid.Parse("a1b2c3d4-e5f6-4a5b-8c9d-0e1f2a3b4c5d");
        var medicine2Id = Guid.Parse("b2c3d4e5-f6a7-4b6c-9d0e-1f2a3b4c5d6e");
        var medicine3Id = Guid.Parse("c3d4e5f6-a7b8-4c7d-0e1f-2a3b4c5d6e7f");
        var medicine4Id = Guid.Parse("d4e5f6a7-b8c9-4d8e-1f2a-3b4c5d6e7f8a");
        var medicine5Id = Guid.Parse("e5f6a7b8-c9d0-4e9f-2a3b-4c5d6e7f8a9b");

        modelBuilder.Entity<Medicine>().HasData(
            new Medicine { Id = medicine1Id, Name = "Panadol Advance", Description = "Paracetamol 500mg for pain relief and fever." },
            new Medicine { Id = medicine2Id, Name = "Amoxil", Description = "Amoxicillin 500mg Broad-spectrum antibiotic." },
            new Medicine { Id = medicine3Id, Name = "Voltaren", Description = "Diclofenac Sodium 50mg Non-steroidal anti-inflammatory drug (NSAID)." },
            new Medicine { Id = medicine4Id, Name = "Lasix", Description = "Furosemide 40mg Diuretic for fluid retention." },
            new Medicine { Id = medicine5Id, Name = "Augmentin", Description = "Co-amoxiclav 1g Antibiotic for bacterial infections." }
        );

        // --- 2. Seed Users (Required for Suppliers and Pharmacies) ---
        var supplierUser1Id = Guid.Parse("f1a2b3c4-d5e6-4a5b-8c9d-0e1f2a3b4c5d");
        var supplierUser2Id = Guid.Parse("f2b3c4d5-e6f7-4b6c-9d0e-1f2a3b4c5d6e");
        var supplierUser3Id = Guid.Parse("f3c4d5e6-f7a8-4c7d-0e1f-2a3b4c5d6e7f");
        var pharmacyUser1Id = Guid.Parse("f4d5e6f7-a8b9-4d8e-1f2a-3b4c5d6e7f8a");
        var pharmacyUser2Id = Guid.Parse("f5e6f7a8-b9c0-4e9f-2a3b-4c5d6e7f8a9b");
        var pharmacyUser3Id = Guid.Parse("f6f7a8b9-c0d1-4f0a-3b4c-5d6e7f8a9b0c");

        modelBuilder.Entity<User>().HasData(
            new User { Id = supplierUser1Id, Name = "Iraqi Pharma Admin", Email = "admin@iqpharma.com", Role = UserRole.Supplier, PasswordHash = "AQAAAAEAACcQAAAAE..." },
            new User { Id = supplierUser2Id, Name = "Pioneer Admin", Email = "admin@pioneer.iq", Role = UserRole.Supplier, PasswordHash = "AQAAAAEAACcQAAAAE..." },
            new User { Id = supplierUser3Id, Name = "Mansour Pharma Admin", Email = "admin@mansour.com", Role = UserRole.Supplier, PasswordHash = "AQAAAAEAACcQAAAAE..." },
            new User { Id = pharmacyUser1Id, Name = "Baghdad Pharmacy Owner", Email = "owner@baghdadpharma.com", Role = UserRole.Pharmacy, PasswordHash = "AQAAAAEAACcQAAAAE..." },
            new User { Id = pharmacyUser2Id, Name = "Erbil Pharmacy Owner", Email = "owner@erbilpharma.com", Role = UserRole.Pharmacy, PasswordHash = "AQAAAAEAACcQAAAAE..." },
            new User { Id = pharmacyUser3Id, Name = "Basra Pharmacy Owner", Email = "owner@basrapharma.com", Role = UserRole.Pharmacy, PasswordHash = "AQAAAAEAACcQAAAAE..." }
        );

        // --- 3. Seed Suppliers ---
        var supplier1Id = Guid.Parse("11111111-2222-3333-4444-555555555555");
        var supplier2Id = Guid.Parse("22222222-3333-4444-5555-666666666666");
        var supplier3Id = Guid.Parse("33333333-4444-5555-6666-777777777777");

        modelBuilder.Entity<Supplier>().HasData(
            new Supplier
            {
                Id = supplier1Id, UserId = supplierUser1Id, Name = "Iraqi Pharmaceutical Industry", Address = "Baghdad, Al-Mansour District",
                Latitude = 33.3152, Longitude = 44.3661, PhoneNumber = "+9647801234567", ServicesDescription = "Local manufacturer and wholesale distributor."
            },
            new Supplier
            {
                Id = supplier2Id, UserId = supplierUser2Id, Name = "Pioneer Pharmaceutical Industries", Address = "Erbil, Industrial Zone",
                Latitude = 36.1911, Longitude = 44.0091, PhoneNumber = "+9647501234567", ServicesDescription = "Import/Export and large scale distribution."
            },
            new Supplier
            {
                Id = supplier3Id, UserId = supplierUser3Id, Name = "Al-Mansour Pharmaceuticals", Address = "Baghdad, Baghdad Al-Jadida",
                Latitude = 33.3248, Longitude = 44.4512, PhoneNumber = "+9647701234567", ServicesDescription = "Specialized in chronic disease medications."
            }
        );

        // --- 4. Seed Pharmacies ---
        modelBuilder.Entity<Pharmacy>().HasData(
            new Pharmacy { Id = Guid.Parse("77777777-1111-2222-3333-444444444444"), UserId = pharmacyUser1Id, Name = "Al-Amal Pharmacy", Latitude = 33.3015, Longitude = 44.4209, PhoneNumber = "+9647811122233" },
            new Pharmacy { Id = Guid.Parse("88888888-1111-2222-3333-444444444444"), UserId = pharmacyUser2Id, Name = "Zheen Pharmacy", Latitude = 36.1833, Longitude = 44.0125, PhoneNumber = "+9647501112223" },
            new Pharmacy { Id = Guid.Parse("99999999-1111-2222-3333-444444444444"), UserId = pharmacyUser3Id, Name = "Al-Fayhaa Pharmacy", Latitude = 30.5081, Longitude = 47.7835, PhoneNumber = "+9647711122233" }
        );

        // --- 5. Seed SupplierMedicines (Links) ---
        modelBuilder.Entity<SupplierMedicine>().HasData(
            // Supplier 1 (Iraqi Pharma) stock
            new SupplierMedicine { SupplierId = supplier1Id, MedicineId = medicine1Id, WholesalePrice = 1250, MinimumOrderQuantity = 10, StockQuantity = 500 },
            new SupplierMedicine { SupplierId = supplier1Id, MedicineId = medicine2Id, WholesalePrice = 3500, MinimumOrderQuantity = 5, StockQuantity = 200 },
            new SupplierMedicine { SupplierId = supplier1Id, MedicineId = medicine5Id, WholesalePrice = 8000, MinimumOrderQuantity = 3, StockQuantity = 100 },

            // Supplier 2 (Pioneer) stock
            new SupplierMedicine { SupplierId = supplier2Id, MedicineId = medicine1Id, WholesalePrice = 1200, MinimumOrderQuantity = 20, StockQuantity = 1000 },
            new SupplierMedicine { SupplierId = supplier2Id, MedicineId = medicine3Id, WholesalePrice = 2500, MinimumOrderQuantity = 10, StockQuantity = 350 },
            
            // Supplier 3 (Al-Mansour) stock
            new SupplierMedicine { SupplierId = supplier3Id, MedicineId = medicine2Id, WholesalePrice = 3600, MinimumOrderQuantity = 5, StockQuantity = 150 },
            new SupplierMedicine { SupplierId = supplier3Id, MedicineId = medicine4Id, WholesalePrice = 1500, MinimumOrderQuantity = 10, StockQuantity = 400 }
        );
    }
}
