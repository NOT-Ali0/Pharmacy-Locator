using Domain.Entities;

namespace Application.Interfaces;

public interface ISupplierRepository : IRepository<Supplier>
{
    Task<Supplier?> GetByIdWithMedicinesAsync(Guid id);
    Task<SupplierMedicine?> GetSupplierMedicineAsync(Guid supplierId, Guid medicineId);
    Task AddSupplierMedicineAsync(SupplierMedicine supplierMedicine);
    Task UpdateSupplierMedicineAsync(SupplierMedicine supplierMedicine);
    Task RemoveSupplierMedicineAsync(SupplierMedicine supplierMedicine);
    Task<IEnumerable<Supplier>> GetAllWithMedicinesAsync();
}
