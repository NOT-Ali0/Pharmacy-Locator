using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces;

public interface IOrderRepository : IRepository<Order>
{
    Task<IEnumerable<Order>> GetByPharmacyIdAsync(Guid pharmacyId);
    Task<IEnumerable<Order>> GetBySupplierIdAsync(Guid supplierId);
    Task<Order?> GetByIdWithItemsAsync(Guid id);
}
