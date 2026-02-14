using Domain.Enums;

namespace Application.DTOs;

public record RegisterRequest(string Name, string Email, string Password, UserRole Role);
public record LoginRequest(string Email, string Password);
public record LoginResponseDto(string Token, UserDto User);
public record UserDto(Guid Id, string Name, string Email, UserRole Role);
//public record PharmacyDto(Guid Id, string Name, double Latitude, double Longitude, string PhoneNumber, List<MedicineDto> Medicines = null!);
public record PharmacyDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Address { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = null!;
    public List<MedicineDto> Medicines { get; set; } = new List<MedicineDto>();
}
public record MedicineDto(Guid Id, string Name, string Description, bool? IsAvailable);
public record SearchMedicineRequest(string MedicineName, string Location);
public record PharmacySearchResultDto(string PharmacyName, string Address, bool Available);

// Supplier DTOs
// public record SupplierDto(Guid Id, string Name, string PhoneNumber, string Address, string ServicesDescription, List<SupplierMedicineDto> Medicines);
public record SupplierDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public string PhoneNumber { get; init; } = null!;
    public string Address { get; init; } = null!;
    public string ServicesDescription { get; init; } = null!;
    public List<SupplierMedicineDto> Medicines { get; init; } = new();
}
public record SupplierMedicineDto(Guid MedicineId, string MedicineName, decimal WholesalePrice, int MinimumOrderQuantity, int StockQuantity);
public record UpdateSupplierMedicineDto(decimal WholesalePrice, int MinimumOrderQuantity, int StockQuantity);
public record AddSupplierMedicineDto(Guid MedicineId, decimal WholesalePrice, int MinimumOrderQuantity, int StockQuantity);

// Order DTOs
public record OrderDto(Guid Id, Guid PharmacyId, string PharmacyName, Guid SupplierId, string SupplierName, DateTime OrderDate, string Status, decimal TotalAmount, List<OrderItemDto> Items);
public record OrderItemDto(Guid MedicineId, string MedicineName, int Quantity, decimal UnitPrice);
public record CreateOrderDto(Guid SupplierId, List<CreateOrderItemDto> Items);
public record CreateOrderItemDto(Guid MedicineId, int Quantity);

