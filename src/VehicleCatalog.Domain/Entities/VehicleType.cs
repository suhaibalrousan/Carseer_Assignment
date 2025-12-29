namespace VehicleCatalog.Domain.Entities;

public class VehicleType
{
    public int VehicleTypeId { get; private set; }
    public string VehicleTypeName { get; private set; }

    public VehicleType(int vehicleTypeId, string vehicleTypeName)
    {
        if (vehicleTypeId <= 0)
            throw new ArgumentException("VehicleTypeId must be greater than zero", nameof(vehicleTypeId));
        
        if (string.IsNullOrWhiteSpace(vehicleTypeName))
            throw new ArgumentException("VehicleTypeName cannot be null or empty", nameof(vehicleTypeName));

        VehicleTypeId = vehicleTypeId;
        VehicleTypeName = vehicleTypeName.Trim();
    }

    private VehicleType() { }
}
