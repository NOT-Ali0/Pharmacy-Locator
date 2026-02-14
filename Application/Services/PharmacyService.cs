using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using AutoMapper;
using Domain.Entities;

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
            
        await _pharmacyRepository.AddAsync(pharmacy);
    }

    public async Task<IEnumerable<PharmacySearchResultDto>> SearchMedicineAsync(SearchMedicineRequest request)
    {
        var pharmacies = await _pharmacyRepository.SearchPharmaciesAsync(request.MedicineName, 3);
        
        return pharmacies.Select(p => new PharmacySearchResultDto(
            p.Name,
            p.Address, 
            true
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
