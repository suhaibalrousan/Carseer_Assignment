using System.Text.Json;
using VehicleCatalog.Infrastructure.Models;

namespace VehicleCatalog.Infrastructure.ExternalServices;

public class NHTSAApiClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public NHTSAApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _httpClient.BaseAddress = new Uri("https://vpic.nhtsa.dot.gov/api/vehicles/");
        
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<NHTSAMakeResponse?> GetAllMakesAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("getallmakes?format=json");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<NHTSAMakeResponse>(content, _jsonOptions);
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException("Failed to retrieve vehicle makes from NHTSA API", ex);
        }
    }

    public async Task<NHTSAVehicleTypeResponse?> GetVehicleTypesByMakeIdAsync(int makeId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"GetVehicleTypesForMakeId/{makeId}?format=json");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<NHTSAVehicleTypeResponse>(content, _jsonOptions);
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException($"Failed to retrieve vehicle types for make ID {makeId}", ex);
        }
    }

    public async Task<NHTSAModelResponse?> GetModelsByMakeIdAndYearAsync(int makeId, int year)
    {
        try
        {
            var response = await _httpClient.GetAsync($"GetModelsForMakeIdYear/makeId/{makeId}/modelyear/{year}?format=json");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<NHTSAModelResponse>(content, _jsonOptions);
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException($"Failed to retrieve models for make ID {makeId} and year {year}", ex);
        }
    }
}
