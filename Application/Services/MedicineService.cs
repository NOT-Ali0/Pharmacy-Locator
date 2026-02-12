using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class MedicineService : IMedicineService
{
    private readonly IRepository<Medicine> _medicineRepository;
    private readonly IMapper _mapper;

    public MedicineService(IRepository<Medicine> medicineRepository, IMapper mapper)
    {
        _medicineRepository = medicineRepository;
        _mapper = mapper;
    }

    public async Task AddMedicineAsync(MedicineDto medicineDto)
    {
        var medicine = _mapper.Map<Medicine>(medicineDto);
        await _medicineRepository.AddAsync(medicine);
    }

    public async Task<IEnumerable<MedicineDto>> GetAllMedicinesAsync()
    {
        var medicines = await _medicineRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<MedicineDto>>(medicines);
    }
}
