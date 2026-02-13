using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces;

public interface IPharmacyService
{
    Task<IEnumerable<PharmacySearchResultDto>> SearchMedicineAsync(SearchMedicineRequest request);
    Task AddPharmacyAsync(PharmacyDto pharmacyDto, Guid userId); // MVP: Simple add
    Task<PharmacyDto?> GetMyPharmacyAsync(Guid userId);
    Task UpdateMedicineAvailabilityAsync(Guid userId, Guid medicineId, bool isAvailable);
    Task DeleteMedicineAsync(Guid userId, Guid medicineId);
}

public interface IMedicineService
{
    Task AddMedicineAsync(MedicineDto medicineDto);
    Task<IEnumerable<MedicineDto>> GetAllMedicinesAsync();
}

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
