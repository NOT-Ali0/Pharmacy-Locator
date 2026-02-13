using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class SupplierRepository : Repository<Supplier>, ISupplierRepository
{
    public SupplierRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Supplier?> GetByIdWithMedicinesAsync(Guid id)
    {
        return await _context.Suppliers
            .Include(s => s.SupplierMedicines)
            .ThenInclude(sm => sm.Medicine)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<SupplierMedicine?> GetSupplierMedicineAsync(Guid supplierId, Guid medicineId)
    {
        return await _context.SupplierMedicines
            .Include(sm => sm.Medicine)
            .FirstOrDefaultAsync(sm => sm.SupplierId == supplierId && sm.MedicineId == medicineId);
    }

    public async Task AddSupplierMedicineAsync(SupplierMedicine supplierMedicine)
    {
        await _context.SupplierMedicines.AddAsync(supplierMedicine);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateSupplierMedicineAsync(SupplierMedicine supplierMedicine)
    {
        _context.SupplierMedicines.Update(supplierMedicine);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveSupplierMedicineAsync(SupplierMedicine supplierMedicine)
    {
        _context.SupplierMedicines.Remove(supplierMedicine);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Supplier>> GetAllWithMedicinesAsync()
    {
        return await _context.Suppliers
            .Include(s => s.SupplierMedicines)
            .ThenInclude(sm => sm.Medicine)
            .ToListAsync();
    }
}
