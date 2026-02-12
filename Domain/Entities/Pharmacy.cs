using NetTopologySuite.Geometries;

namespace Domain.Entities;

public class Pharmacy
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public Point Location { get; set; } = null!; // PostGIS Point
    public string PhoneNumber { get; set; } = string.Empty;

    public User User { get; set; } = null!;
    public ICollection<PharmacyMedicine> PharmacyMedicines { get; set; } = new List<PharmacyMedicine>();
}
