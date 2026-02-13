using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using NetTopologySuite.Geometries;
using System.Linq;

namespace Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterRequest, User>();
        CreateMap<Pharmacy, PharmacyDto>()
            .ForMember(d => d.Latitude, opt => opt.MapFrom(s => s.Location.Y))
            .ForMember(d => d.Longitude, opt => opt.MapFrom(s => s.Location.X))
            .ForMember(d => d.Medicines, opt => opt.MapFrom(s => s.PharmacyMedicines));
            
        CreateMap<PharmacyDto, Pharmacy>()
            .ForMember(d => d.Location, opt => opt.MapFrom(s => new Point(s.Longitude, s.Latitude) { SRID = 4326 }));

        CreateMap<Pharmacy, PharmacyDto>()
    .ForMember(d => d.Latitude, opt => opt.MapFrom(s => s.Location.Y))
    .ForMember(d => d.Longitude, opt => opt.MapFrom(s => s.Location.X))
    .ForMember(d => d.Medicines, opt => opt.MapFrom(s => s.PharmacyMedicines.Select(pm => new MedicineDto(pm.Medicine.Id, pm.Medicine.Name,pm.Medicine.Description, pm.IsAvailable))));


        CreateMap<Medicine, MedicineDto>().ReverseMap();
        
        CreateMap<PharmacyMedicine, MedicineDto>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.MedicineId))
            .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Medicine.Name))
            .ForMember(d => d.Description, opt => opt.MapFrom(s => s.Medicine.Description))
            .ForMember(d => d.IsAvailable, opt => opt.MapFrom(s => s.IsAvailable));
    }
}
