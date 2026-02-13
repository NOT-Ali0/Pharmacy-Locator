using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Order>> GetByPharmacyIdAsync(Guid pharmacyId)
    {
        return await _context.Orders
            .Include(o => o.Supplier)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Medicine)
            .Where(o => o.PharmacyId == pharmacyId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetBySupplierIdAsync(Guid supplierId)
    {
        return await _context.Orders
            .Include(o => o.Pharmacy)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Medicine)
            .Where(o => o.SupplierId == supplierId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    public async Task<Order?> GetByIdWithItemsAsync(Guid id)
    {
        return await _context.Orders
            .Include(o => o.Pharmacy)
            .Include(o => o.Supplier)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Medicine)
            .FirstOrDefaultAsync(o => o.Id == id);
    }
}
