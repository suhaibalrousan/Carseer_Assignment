using VehicleCatalog.Domain.Entities;

namespace VehicleCatalog.Domain.Interfaces;

public interface IVehicleRepository
{
    /// <summary>
    /// Gets all vehicle makes from the NHTSA database
    /// </summary>
    Task<IEnumerable<VehicleMake>> GetAllMakesAsync();

    /// <summary>
    /// Gets vehicle types for a specific make
    /// </summary>
    /// <param name="makeId">The make identifier</param>
    Task<IEnumerable<VehicleType>> GetVehicleTypesByMakeIdAsync(int makeId);

    /// <summary>
    /// Gets vehicle models for a specific make and year
    /// </summary>
    /// <param name="makeId">The make identifier</param>
    /// <param name="year">The model year</param>
    Task<IEnumerable<VehicleModel>> GetModelsByMakeIdAndYearAsync(int makeId, int year);
}
