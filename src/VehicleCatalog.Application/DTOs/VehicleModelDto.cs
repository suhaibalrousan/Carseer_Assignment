namespace VehicleCatalog.Application.DTOs;

public class VehicleModelDto
{
    public int MakeId { get; set; }
    public int ModelId { get; set; }
    public string ModelName { get; set; } = string.Empty;
    public int ModelYear { get; set; }
}
