using Domain.Enums;

namespace Application.DTOs;

public record RegisterRequest(string Name, string Email, string Password, UserRole Role);
public record LoginRequest(string Email, string Password);
public record PharmacyDto(Guid Id, string Name, double Latitude, double Longitude, string PhoneNumber);
public record MedicineDto(Guid Id, string Name, string Description);
public record SearchMedicineRequest(string MedicineName, double UserLat, double UserLng);
public record PharmacySearchResultDto(string PharmacyName, string Address, double Distance, bool Available);
