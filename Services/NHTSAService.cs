using System.Text.Json;
using VehicleCatalog.Web.Models;

namespace VehicleCatalog.Web.Services
{
    public interface INHTSAService
    {
        Task<List<VehicleMake>> GetAllMakesAsync();
        Task<List<VehicleType>> GetVehicleTypesForMakeAsync(int makeId);
        Task<List<VehicleModel>> GetModelsForMakeAndYearAsync(int makeId, int year);
    }

    public class NHTSAService : INHTSAService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<NHTSAService> _logger;
        private const string BaseUrl = "https://vpic.nhtsa.dot.gov/api/vehicles";

        public NHTSAService(HttpClient httpClient, ILogger<NHTSAService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<VehicleMake>> GetAllMakesAsync()
        {
            try
            {
                var url = $"{BaseUrl}/getallmakes?format=json";
                var response = await _httpClient.GetFromJsonAsync<NHTSAApiResponse<VehicleMake>>(url);
                
                if (response?.Results != null)
                {
                    _logger.LogInformation($"Retrieved {response.Count} vehicle makes from NHTSA API");
                    return response.Results.OrderBy(m => m.Make_Name).ToList();
                }
                
                _logger.LogWarning("No results returned from NHTSA API for makes");
                return new List<VehicleMake>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching vehicle makes from NHTSA API");
                throw new ApplicationException("Failed to retrieve vehicle makes. Please try again later.", ex);
            }
        }

        public async Task<List<VehicleType>> GetVehicleTypesForMakeAsync(int makeId)
        {
            try
            {
                var url = $"{BaseUrl}/GetVehicleTypesForMakeId/{makeId}?format=json";
                var response = await _httpClient.GetFromJsonAsync<NHTSAApiResponse<VehicleType>>(url);
                
                if (response?.Results != null)
                {
                    _logger.LogInformation($"Retrieved {response.Count} vehicle types for make ID {makeId}");
                    return response.Results;
                }
                
                _logger.LogWarning($"No vehicle types found for make ID {makeId}");
                return new List<VehicleType>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching vehicle types for make ID {makeId}");
                throw new ApplicationException("Failed to retrieve vehicle types. Please try again later.", ex);
            }
        }

        public async Task<List<VehicleModel>> GetModelsForMakeAndYearAsync(int makeId, int year)
        {
            try
            {
                var url = $"{BaseUrl}/GetModelsForMakeIdYear/makeId/{makeId}/modelyear/{year}?format=json";
                var response = await _httpClient.GetFromJsonAsync<NHTSAApiResponse<VehicleModel>>(url);
                
                if (response?.Results != null)
                {
                    _logger.LogInformation($"Retrieved {response.Count} models for make ID {makeId} and year {year}");
                    return response.Results.OrderBy(m => m.Model_Name).ToList();
                }
                
                _logger.LogWarning($"No models found for make ID {makeId} and year {year}");
                return new List<VehicleModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching models for make ID {makeId} and year {year}");
                throw new ApplicationException("Failed to retrieve vehicle models. Please try again later.", ex);
            }
        }
    }
}
