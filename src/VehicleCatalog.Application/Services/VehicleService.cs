using AutoMapper;
using VehicleCatalog.Application.DTOs;
using VehicleCatalog.Domain.Interfaces;

namespace VehicleCatalog.Application.Services;

public class VehicleService
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IMapper _mapper;

    public VehicleService(IVehicleRepository vehicleRepository, IMapper mapper)
    {
        _vehicleRepository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<VehicleMakeDto>> GetAllMakesAsync()
    {
        var makes = await _vehicleRepository.GetAllMakesAsync();
        return _mapper.Map<IEnumerable<VehicleMakeDto>>(makes);
    }

    public async Task<IEnumerable<VehicleTypeDto>> GetVehicleTypesAsync(int makeId)
    {
        if (makeId <= 0)
            throw new ArgumentException("MakeId must be greater than zero", nameof(makeId));

        var types = await _vehicleRepository.GetVehicleTypesByMakeIdAsync(makeId);
        return _mapper.Map<IEnumerable<VehicleTypeDto>>(types);
    }

    public async Task<IEnumerable<VehicleModelDto>> GetModelsAsync(int makeId, int year)
    {
        if (makeId <= 0)
            throw new ArgumentException("MakeId must be greater than zero", nameof(makeId));

        if (year < 1900 || year > DateTime.Now.Year + 2)
            throw new ArgumentException($"Year must be between 1900 and {DateTime.Now.Year + 2}", nameof(year));

        var models = await _vehicleRepository.GetModelsByMakeIdAndYearAsync(makeId, year);
        return _mapper.Map<IEnumerable<VehicleModelDto>>(models);
    }
}
