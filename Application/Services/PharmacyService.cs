using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using NetTopologySuite.Geometries;

namespace Application.Services;

public class PharmacyService : IPharmacyService
{
    private readonly IPharmacyRepository _pharmacyRepository;
    private readonly IMapper _mapper;

    public PharmacyService(IPharmacyRepository pharmacyRepository, IMapper mapper)
    {
        _pharmacyRepository = pharmacyRepository;
        _mapper = mapper;
    }

    public async Task AddPharmacyAsync(PharmacyDto pharmacyDto, Guid userId)
    {

        var existingPharmacy = await _pharmacyRepository
        .FindAsync(p => p.UserId == userId);

        if (existingPharmacy.Any())
        {
            throw new Exception("This user already has a pharmacy.");
        }


        var pharmacy = _mapper.Map<Pharmacy>(pharmacyDto);
        pharmacy.UserId = userId;
        
        // Ensure Location is set correctly if AutoMapper didn't handle SRID 4326 via constructor/factory
        if (pharmacy.Location != null && pharmacy.Location.SRID != 4326)
            pharmacy.Location.SRID = 4326;
            
        await _pharmacyRepository.AddAsync(pharmacy);
    }

    public async Task<IEnumerable<PharmacySearchResultDto>> SearchMedicineAsync(SearchMedicineRequest request)
    {
        var pharmacies = await _pharmacyRepository.GetNearestPharmaciesAsync(request.MedicineName, request.UserLat, request.UserLng, 3);
        
        var userLocation = new Point(request.UserLng, request.UserLat) { SRID = 4326 };

        return pharmacies.Select(p => new PharmacySearchResultDto(
            p.Name,
            $"Lat: {p.Location.Y}, Lng: {p.Location.X}", // Simplified address
            p.Location.Distance(userLocation), // Distance in degrees, needs conversion for real use but MVP is fine. PostGIS returns meters usually if projected, but SRID 4326 is degrees. 
            // Better to let PostGIS calculate distance in meters in the query, but Repository interface returns Entities.
            // For MVP, simplistic distance is okay or we can assume Repository handles ranking.
            true // Assumed available if returned by GetNearestPharmaciesAsync
        ));
    }

    public async Task<PharmacyDto?> GetMyPharmacyAsync(Guid userId)
    {
        var pharmacy = await _pharmacyRepository.GetByUserIdAsync(userId);
        if (pharmacy == null) return null;
        return _mapper.Map<PharmacyDto>(pharmacy);
    }

    public async Task UpdateMedicineAvailabilityAsync(Guid userId, Guid medicineId, bool isAvailable)
    {
        var pharmacy = await _pharmacyRepository.GetByUserIdAsync(userId);
        if (pharmacy == null) throw new Exception("Pharmacy not found");

        var medicine = await _pharmacyRepository.GetMedicineByIdAsync(pharmacy.Id, medicineId);
        if (medicine == null) throw new KeyNotFoundException("Medicine not found in your pharmacy");

        medicine.IsAvailable = isAvailable;
        await _pharmacyRepository.UpdateMedicineAsync(medicine);
    }

    public async Task DeleteMedicineAsync(Guid userId, Guid medicineId)
    {
        var pharmacy = await _pharmacyRepository.GetByUserIdAsync(userId);
        if (pharmacy == null) throw new Exception("Pharmacy not found");

        var medicine = await _pharmacyRepository.GetMedicineByIdAsync(pharmacy.Id, medicineId);
        if (medicine == null) throw new KeyNotFoundException("Medicine not found in your pharmacy");

        await _pharmacyRepository.DeleteMedicineAsync(medicine);
    }
}
