using Application.DTOs;

namespace Application.Interfaces;

public interface ISupplierService
{
    Task<SupplierDto> CreateSupplierAsync(SupplierDto supplierDto); // Or implicit from user creation? Assuming manual for now or via endpoint
    // Actually requirement says "Add Medicine", implies Supplier exists.
    // We probably need a way to link User to Supplier? Or Supplier IS a User role?
    // Requirement "UserId" updates on Pharmacy implies Supplier might also have UserId or be linked. 
    // But requirement "Add Supplier Entity" didn't explicitly say "Add UserId to Supplier". 
    // However, "Ensure Supplier can only manage his own medicines" implies auth/ownership.
    // I will add UserId to Supplier entity in the next step or assume it should be there.
    // Wait, the plan check "Add `Supplier` Entity" -> "Create `Supplier` class with Id...". 
    // I missed `UserId` in `Supplier` entity creation if it's needed for ownership.
    // "Ensure Supplier can only manage his own medicines" -> Strongly implies `UserId`.
    // I will add `UserId` to Supplier entity first.
    
    Task<SupplierDto?> GetSupplierByUserIdAsync(Guid userId);
    Task<IEnumerable<SupplierDto>> GetAllSuppliersAsync();
    Task AddMedicineAsync(Guid supplierId, AddSupplierMedicineDto dto);
    Task UpdateMedicineAsync(Guid supplierId, Guid medicineId, UpdateSupplierMedicineDto dto);
    Task RemoveMedicineAsync(Guid supplierId, Guid medicineId);
    Task<IEnumerable<OrderDto>> GetOrdersAsync(Guid supplierId); // For Supplier to view
    Task UpdateOrderStatusAsync(Guid supplierId, Guid orderId, string status); // Approve/Reject
}
