using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using System.Linq;

namespace Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterRequest, User>();
        CreateMap<Pharmacy, PharmacyDto>()
            .ForMember(d => d.Medicines, opt => opt.MapFrom(s => s.PharmacyMedicines));

        CreateMap<PharmacyDto, Pharmacy>()
            .ForMember(d => d.Id, opt => opt.Ignore()) // Do not map ID if it's generated, or map if provided. Usually DTO -> Entity creation ignores ID.
            .ForMember(d => d.PharmacyMedicines, opt => opt.Ignore())
            .ForMember(d => d.User, opt => opt.Ignore());

        CreateMap<Medicine, MedicineDto>()
            .ForCtorParam("Id", opt => opt.MapFrom(s => s.Id))
            .ForCtorParam("Name", opt => opt.MapFrom(s => s.Name))
            .ForCtorParam("Description", opt => opt.MapFrom(s => s.Description))
            .ForCtorParam("IsAvailable", opt => opt.MapFrom(s => (bool?)null))
            .ReverseMap();
        
        CreateMap<PharmacyMedicine, MedicineDto>()
            .ForCtorParam("Id", opt => opt.MapFrom(s => s.MedicineId))
            .ForCtorParam("Name", opt => opt.MapFrom(s => s.Medicine.Name))
            .ForCtorParam("Description", opt => opt.MapFrom(s => s.Medicine.Description))
            .ForCtorParam("IsAvailable", opt => opt.MapFrom(s => s.IsAvailable));

        // Supplier Mappings
        CreateMap<Supplier, SupplierDto>()
            .ForMember(d => d.Medicines, opt => opt.MapFrom(s => s.SupplierMedicines));
            
        CreateMap<SupplierMedicine, SupplierMedicineDto>()
            .ForCtorParam("MedicineId", opt => opt.MapFrom(s => s.MedicineId))
            .ForCtorParam("MedicineName", opt => opt.MapFrom(s => s.Medicine.Name))
            .ForCtorParam("WholesalePrice", opt => opt.MapFrom(s => s.WholesalePrice))
            .ForCtorParam("MinimumOrderQuantity", opt => opt.MapFrom(s => s.MinimumOrderQuantity))
            .ForCtorParam("StockQuantity", opt => opt.MapFrom(s => s.StockQuantity));

        // Order Mappings
        CreateMap<Order, OrderDto>()
            .ForMember(d => d.PharmacyName, opt => opt.MapFrom(s => s.Pharmacy.Name))
            .ForMember(d => d.SupplierName, opt => opt.MapFrom(s => s.Supplier.Name))
            .ForMember(d => d.Items, opt => opt.MapFrom(s => s.OrderItems));

        CreateMap<OrderItem, OrderItemDto>()
            .ForCtorParam("MedicineId", opt => opt.MapFrom(s => s.MedicineId))
            .ForCtorParam("MedicineName", opt => opt.MapFrom(s => s.Medicine.Name))
            .ForCtorParam("Quantity", opt => opt.MapFrom(s => s.Quantity))
            .ForCtorParam("UnitPrice", opt => opt.MapFrom(s => s.UnitPrice));
    }
}
