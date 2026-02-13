namespace Domain.Entities;

public class OrderItem
{
    public Guid OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public Guid MedicineId { get; set; }
    public Medicine Medicine { get; set; } = null!;

    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
