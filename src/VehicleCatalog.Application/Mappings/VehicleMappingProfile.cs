using AutoMapper;
using VehicleCatalog.Application.DTOs;
using VehicleCatalog.Domain.Entities;

namespace VehicleCatalog.Application.Mappings;

public class VehicleMappingProfile : Profile
{
    public VehicleMappingProfile()
    {
        CreateMap<VehicleMake, VehicleMakeDto>();
        CreateMap<VehicleType, VehicleTypeDto>();
        CreateMap<VehicleModel, VehicleModelDto>();
    }
}
