namespace Domain.Entities;

public class PharmacyMedicine
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PharmacyId { get; set; }
    public Guid MedicineId { get; set; }
    public bool IsAvailable { get; set; }
    public decimal Price { get; set; } // Optional: Add price as it makes sense

    public Pharmacy Pharmacy { get; set; } = null!;
    public Medicine Medicine { get; set; } = null!;
}
