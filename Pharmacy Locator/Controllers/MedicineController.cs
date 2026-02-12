using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Pharmacy_Locator.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MedicineController : ControllerBase
{
    private readonly IMedicineService _medicineService;

    public MedicineController(IMedicineService medicineService)
    {
        _medicineService = medicineService;
    }

    [HttpPost]
    [Authorize(Roles = "Pharmacy")]
    public async Task<IActionResult> AddMedicine(MedicineDto dto)
    {
        await _medicineService.AddMedicineAsync(dto);
        return Ok();
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var medicines = await _medicineService.GetAllMedicinesAsync();
        return Ok(medicines);
    }
}
