using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VehicleCatalog.Application.Services;
using VehicleCatalog.Web.Models;

namespace VehicleCatalog.Web.Controllers;

public class HomeController : Controller
{
    private readonly VehicleService _vehicleService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(VehicleService vehicleService, ILogger<HomeController> logger)
    {
        _vehicleService = vehicleService ?? throw new ArgumentNullException(nameof(vehicleService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var makes = await _vehicleService.GetAllMakesAsync();
            var viewModel = new VehicleSearchViewModel
            {
                Makes = makes.OrderBy(m => m.MakeName).ToList()
            };
            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading vehicle makes");
            return View(new VehicleSearchViewModel());
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetVehicleTypes(int makeId)
    {
        try
        {
            if (makeId <= 0)
                return BadRequest(new { error = "Invalid make ID" });

            var types = await _vehicleService.GetVehicleTypesAsync(makeId);
            return Json(types);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting vehicle types for make ID {MakeId}", makeId);
            return StatusCode(500, new { error = "Failed to retrieve vehicle types" });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetModels(int makeId, int year)
    {
        try
        {
            if (makeId <= 0)
                return BadRequest(new { error = "Invalid make ID" });

            if (year < 1900 || year > DateTime.Now.Year + 2)
                return BadRequest(new { error = $"Invalid year. Must be between 1900 and {DateTime.Now.Year + 2}" });

            var models = await _vehicleService.GetModelsAsync(makeId, year);
            return Json(models);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting models for make ID {MakeId} and year {Year}", makeId, year);
            return StatusCode(500, new { error = "Failed to retrieve vehicle models" });
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
