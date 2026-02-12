using Domain.Entities;

namespace Application.Interfaces;

public interface IPharmacyRepository : IRepository<Pharmacy>
{
    Task<IEnumerable<Pharmacy>> GetNearestPharmaciesAsync(string medicineName, double lat, double lng, int limit = 3);
}
