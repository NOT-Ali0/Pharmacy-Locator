using AutoMapper;
using Application.DTOs;
using Domain.Entities;
using NetTopologySuite.Geometries;

namespace Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterRequest, User>();
        CreateMap<Pharmacy, PharmacyDto>()
            .ForMember(d => d.Latitude, opt => opt.MapFrom(s => s.Location.Y))
            .ForMember(d => d.Longitude, opt => opt.MapFrom(s => s.Location.X));
            
        CreateMap<PharmacyDto, Pharmacy>()
            .ForMember(d => d.Location, opt => opt.MapFrom(s => new Point(s.Longitude, s.Latitude) { SRID = 4326 }));

        CreateMap<Medicine, MedicineDto>().ReverseMap();
    }
}
