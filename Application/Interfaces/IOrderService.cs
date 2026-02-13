using Application.DTOs;

namespace Application.Interfaces;

public interface IOrderService
{
    Task<OrderDto> CreateOrderAsync(Guid pharmacyId, CreateOrderDto dto);
    Task<IEnumerable<OrderDto>> GetPharmacyOrdersAsync(Guid pharmacyId);
    Task<OrderDto?> GetOrderByIdAsync(Guid orderId);
    // Supplier side orders are in SupplierService or here? 
    // "Supplier: View Orders" -> Could be here GetSupplierOrdersAsync
    Task<IEnumerable<OrderDto>> GetSupplierOrdersAsync(Guid supplierId);
    Task UpdateOrderStatusAsync(Guid orderId, string status); // Helper or main logic
}
