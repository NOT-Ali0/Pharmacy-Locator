using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace Infrastructure.Repositories;

public class PharmacyRepository : Repository<Pharmacy>, IPharmacyRepository
{
    public PharmacyRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Pharmacy>> GetNearestPharmaciesAsync(string medicineName, double lat, double lng, int limit = 3)
    {
        var userLocation = new Point(lng, lat) { SRID = 4326 };

        // Logic:
        // 1. Find pharmacies that have the medicine available.
        // 2. Order by distance from user location.
        // 3. Take top 'limit'.
        
        // Note: PostGIS ST_Distance is automatically used by EF Core when accessing .Distance() on geometry types.
        // SRID 4326 (WGS 84) distance is in degrees. For accurate meters, we should project to 3857 or similar, 
        // but for "nearest" sorting, degrees is sufficient as it is monotonic for small areas. 
        // Or better, assume global usage and minimal distortion is acceptable for MVP.
        
        return await _context.Pharmacies
            .Where(p => p.PharmacyMedicines.Any(pm => pm.Medicine.Name.ToLower().Contains(medicineName.ToLower()) && pm.IsAvailable))
            .OrderBy(p => p.Location.Distance(userLocation))
            .Take(limit)
            .ToListAsync();
    }

    public async Task<Pharmacy?> GetByUserIdAsync(Guid userId)
    {
        return await _context.Pharmacies
            .Include(p => p.PharmacyMedicines)
            .ThenInclude(pm => pm.Medicine)
            .FirstOrDefaultAsync(p => p.UserId == userId);
    }

    public async Task<PharmacyMedicine?> GetMedicineByIdAsync(Guid pharmacyId, Guid medicineId)
    {
        return await _context.Set<PharmacyMedicine>()
            .FirstOrDefaultAsync(pm => pm.PharmacyId == pharmacyId && pm.MedicineId == medicineId);
    }

    public async Task UpdateMedicineAsync(PharmacyMedicine pharmacyMedicine)
    {
        _context.Set<PharmacyMedicine>().Update(pharmacyMedicine);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteMedicineAsync(PharmacyMedicine pharmacyMedicine)
    {
        _context.Set<PharmacyMedicine>().Remove(pharmacyMedicine);
        await _context.SaveChangesAsync();
    }
}
