using Domain.Entities;

namespace Domain.Entities;

public class SupplierMedicine
{
    public Guid SupplierId { get; set; }
    public Supplier Supplier { get; set; } = null!;

    public Guid MedicineId { get; set; }
    public Medicine Medicine { get; set; } = null!;

    public decimal WholesalePrice { get; set; }
    public int MinimumOrderQuantity { get; set; }
    public int StockQuantity { get; set; }
}
