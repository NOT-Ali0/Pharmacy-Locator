using Application.DTOs;
using Application.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Pharmacy_Locator.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = nameof(UserRole.Supplier))]
public class SupplierController : ControllerBase
{
    private readonly ISupplierService _supplierService;
    private readonly IOrderService _orderService;

    public SupplierController(ISupplierService supplierService, IOrderService orderService)
    {
        _supplierService = supplierService;
        _orderService = orderService;
    }

    private Guid GetUserId() => Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

    [HttpGet("me")]
    public async Task<IActionResult> GetMySupplier()
    {
        var supplier = await _supplierService.GetSupplierByUserIdAsync(GetUserId());
        if (supplier == null) return NotFound("Supplier profile not found.");
        return Ok(supplier);
    }
    
    // TODO: Endpoint to create supplier profile if not exists? Or assumed pre-created?
    // Requirement "Add Supplier Entity" implies we might need to create it.
    // "Supplier: Add Medicine" implies profile exists.
    
    [HttpPost("medicines")]
    public async Task<IActionResult> AddMedicine(AddSupplierMedicineDto dto)
    {
        var supplier = await _supplierService.GetSupplierByUserIdAsync(GetUserId());
        if (supplier == null) return NotFound("Supplier profile not found.");

        try
        {
            await _supplierService.AddMedicineAsync(supplier.Id, dto);
            return Ok("Medicine added to catalog.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("medicines/{medicineId}")]
    public async Task<IActionResult> UpdateMedicine(Guid medicineId, UpdateSupplierMedicineDto dto)
    {
        var supplier = await _supplierService.GetSupplierByUserIdAsync(GetUserId());
        if (supplier == null) return NotFound("Supplier profile not found.");

        try
        {
            await _supplierService.UpdateMedicineAsync(supplier.Id, medicineId, dto);
            return Ok("Medicine updated.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpDelete("medicines/{medicineId}")]
    public async Task<IActionResult> RemoveMedicine(Guid medicineId)
    {
        var supplier = await _supplierService.GetSupplierByUserIdAsync(GetUserId());
        if (supplier == null) return NotFound("Supplier profile not found.");

        try
        {
            await _supplierService.RemoveMedicineAsync(supplier.Id, medicineId);
            return Ok("Medicine removed.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("orders")]
    public async Task<IActionResult> GetOrders()
    {
        var supplier = await _supplierService.GetSupplierByUserIdAsync(GetUserId());
        if (supplier == null) return NotFound("Supplier profile not found.");

        var orders = await _supplierService.GetOrdersAsync(supplier.Id);
        return Ok(orders);
    }

    [HttpPut("orders/{orderId}/status")]
    public async Task<IActionResult> UpdateOrderStatus(Guid orderId, [FromBody] string status)
    {
        var supplier = await _supplierService.GetSupplierByUserIdAsync(GetUserId());
        if (supplier == null) return NotFound("Supplier profile not found.");

        try
        {
            await _supplierService.UpdateOrderStatusAsync(supplier.Id, orderId, status);
            return Ok($"Order status updated to {status}.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
