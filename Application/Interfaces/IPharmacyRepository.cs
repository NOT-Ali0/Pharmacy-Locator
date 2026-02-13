using Domain.Entities;

namespace Application.Interfaces;

public interface IPharmacyRepository : IRepository<Pharmacy>
{
    Task<IEnumerable<Pharmacy>> SearchPharmaciesAsync(string medicineName, int limit = 10);
    Task<Pharmacy?> GetByUserIdAsync(Guid userId);
    Task<PharmacyMedicine?> GetMedicineByIdAsync(Guid pharmacyId, Guid medicineId);
    Task UpdateMedicineAsync(PharmacyMedicine pharmacyMedicine);
    Task DeleteMedicineAsync(PharmacyMedicine pharmacyMedicine);
}
