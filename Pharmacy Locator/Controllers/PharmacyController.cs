using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Pharmacy_Locator.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PharmacyController : ControllerBase
{
    private readonly IPharmacyService _pharmacyService;

    public PharmacyController(IPharmacyService pharmacyService)
    {
        _pharmacyService = pharmacyService;
    }

    [HttpPost]
    [Authorize(Roles = "Pharmacy")]
    public async Task<IActionResult> CreatePharmacy([FromBody] PharmacyDto dto)
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (Guid.TryParse(userIdString, out Guid userId))
        {
            await _pharmacyService.AddPharmacyAsync(dto, userId);
            return Ok();
        }
        return Unauthorized();
    }

    [HttpGet("search")]
    [Authorize] 
    public async Task<IActionResult> Search([FromQuery] SearchMedicineRequest request)
    {
        var results = await _pharmacyService.SearchMedicineAsync(request);
        return Ok(results);
    }

    [HttpGet("me")]
    [Authorize(Roles = "Pharmacy")]
    public async Task<IActionResult> GetMyPharmacy()
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (Guid.TryParse(userIdString, out Guid userId))
        {
            var pharmacy = await _pharmacyService.GetMyPharmacyAsync(userId);
            if (pharmacy == null) return NotFound();
            return Ok(pharmacy);
        }
        return Unauthorized();
    }
    [HttpPut("medicines/{id}")]
    [Authorize(Roles = "Pharmacy")]
    public async Task<IActionResult> UpdateMedicineAvailability(Guid id, [FromBody] bool isAvailable)
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (Guid.TryParse(userIdString, out Guid userId))
        {
            try
            {
                await _pharmacyService.UpdateMedicineAvailabilityAsync(userId, id, isAvailable);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
        return Unauthorized();
    }

    [HttpDelete("medicines/{id}")]
    [Authorize(Roles = "Pharmacy")]
    public async Task<IActionResult> DeleteMedicine(Guid id)
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (Guid.TryParse(userIdString, out Guid userId))
        {
            try
            {
                await _pharmacyService.DeleteMedicineAsync(userId, id);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
        return Unauthorized();
    }
}
