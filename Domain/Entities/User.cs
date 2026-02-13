using Domain.Enums;

namespace Domain.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.Pharmacy;
    
    // Navigation property for Pharmacy if the user is a pharmacy owner
    // Navigation property for Pharmacy if the user is a pharmacy owner
    public Pharmacy? Pharmacy { get; set; }
    
    // Navigation property for Supplier if the user is a supplier
    public Supplier? Supplier { get; set; }
}
