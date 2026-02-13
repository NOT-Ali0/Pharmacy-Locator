using Domain.Entities;

namespace Application.Interfaces;

public interface IPharmacyRepository : IRepository<Pharmacy>
{
    Task<IEnumerable<Pharmacy>> GetNearestPharmaciesAsync(string medicineName, double lat, double lng, int limit = 3);
    Task<Pharmacy?> GetByUserIdAsync(Guid userId);
    Task<PharmacyMedicine?> GetMedicineByIdAsync(Guid pharmacyId, Guid medicineId);
    Task UpdateMedicineAsync(PharmacyMedicine pharmacyMedicine);
    Task DeleteMedicineAsync(PharmacyMedicine pharmacyMedicine);
}
