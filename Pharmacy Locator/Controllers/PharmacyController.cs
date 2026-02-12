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
}
