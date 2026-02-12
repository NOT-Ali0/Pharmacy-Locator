namespace Domain.Entities;

public class Medicine
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public ICollection<PharmacyMedicine> PharmacyMedicines { get; set; } = new List<PharmacyMedicine>();
}
