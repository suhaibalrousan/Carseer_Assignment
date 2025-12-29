using VehicleCatalog.Application.DTOs;

namespace VehicleCatalog.Web.Models;

public class VehicleSearchViewModel
{
    public List<VehicleMakeDto> Makes { get; set; } = new();
    public int? SelectedMakeId { get; set; }
    public int? Year { get; set; }
}
