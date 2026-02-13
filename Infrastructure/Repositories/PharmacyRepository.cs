using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories;

public class PharmacyRepository : Repository<Pharmacy>, IPharmacyRepository
{
    public PharmacyRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Pharmacy>> SearchPharmaciesAsync(string medicineName, int limit = 10)
    {
        //return await _context.Pharmacies
        //    .Where(p => p.PharmacyMedicines.Any(pm => pm.Medicine.Name.ToLower().Contains(medicineName.ToLower()) && pm.IsAvailable))
        //    .Take(limit)
        //    .ToListAsync();
        return await _context.Pharmacies
        .Include(p => p.PharmacyMedicines)
            .ThenInclude(pm => pm.Medicine)
        .Where(p => p.PharmacyMedicines.Any(pm =>
            pm.Medicine.Name.ToLower().Contains(medicineName.ToLower())
            && pm.IsAvailable))
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
