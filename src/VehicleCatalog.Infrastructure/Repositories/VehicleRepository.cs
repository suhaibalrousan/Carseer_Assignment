using Microsoft.Extensions.Caching.Memory;
using VehicleCatalog.Domain.Entities;
using VehicleCatalog.Domain.Interfaces;
using VehicleCatalog.Infrastructure.ExternalServices;

namespace VehicleCatalog.Infrastructure.Repositories;

public class VehicleRepository : IVehicleRepository
{
    private readonly NHTSAApiClient _apiClient;
    private readonly IMemoryCache _cache;
    private const int CacheDurationMinutes = 60;

    public VehicleRepository(NHTSAApiClient apiClient, IMemoryCache cache)
    {
        _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    public async Task<IEnumerable<VehicleMake>> GetAllMakesAsync()
    {
        const string cacheKey = "all_makes";

        if (_cache.TryGetValue(cacheKey, out IEnumerable<VehicleMake>? cachedMakes) && cachedMakes != null)
        {
            return cachedMakes;
        }

        var response = await _apiClient.GetAllMakesAsync();
        
        if (response?.Results == null || !response.Results.Any())
        {
            return Enumerable.Empty<VehicleMake>();
        }

        var makes = response.Results
            .Select(m => new VehicleMake(m.MakeId, m.MakeName))
            .ToList();

        _cache.Set(cacheKey, makes, TimeSpan.FromMinutes(CacheDurationMinutes));

        return makes;
    }

    public async Task<IEnumerable<VehicleType>> GetVehicleTypesByMakeIdAsync(int makeId)
    {
        var cacheKey = $"vehicle_types_{makeId}";

        if (_cache.TryGetValue(cacheKey, out IEnumerable<VehicleType>? cachedTypes) && cachedTypes != null)
        {
            return cachedTypes;
        }

        var response = await _apiClient.GetVehicleTypesByMakeIdAsync(makeId);
        
        if (response?.Results == null || !response.Results.Any())
        {
            return Enumerable.Empty<VehicleType>();
        }

        var types = response.Results
            .Select(t => new VehicleType(t.VehicleTypeId, t.VehicleTypeName))
            .ToList();

        _cache.Set(cacheKey, types, TimeSpan.FromMinutes(CacheDurationMinutes));

        return types;
    }

    public async Task<IEnumerable<VehicleModel>> GetModelsByMakeIdAndYearAsync(int makeId, int year)
    {
        var cacheKey = $"vehicle_models_{makeId}_{year}";

        if (_cache.TryGetValue(cacheKey, out IEnumerable<VehicleModel>? cachedModels) && cachedModels != null)
        {
            return cachedModels;
        }

        var response = await _apiClient.GetModelsByMakeIdAndYearAsync(makeId, year);
        
        if (response?.Results == null || !response.Results.Any())
        {
            return Enumerable.Empty<VehicleModel>();
        }

        var models = response.Results
            .Where(m => !string.IsNullOrWhiteSpace(m.ModelName)) // Filter out empty model names
            .Select(m => new VehicleModel(m.MakeId, m.ModelId, m.ModelName, year))
            .ToList();

        _cache.Set(cacheKey, models, TimeSpan.FromMinutes(CacheDurationMinutes));

        return models;
    }
}
