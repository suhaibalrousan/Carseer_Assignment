namespace VehicleCatalog.Domain.Entities;

public class VehicleMake
{
    public int MakeId { get; private set; }
    public string MakeName { get; private set; }

 public VehicleMake(int makeId, string makeName)
    {
        if (makeId <= 0)
            throw new ArgumentException("MakeId must be greater than zero", nameof(makeId));
        
        if (string.IsNullOrWhiteSpace(makeName))
            throw new ArgumentException("MakeName cannot be null or empty", nameof(makeName));

        MakeId = makeId;
        MakeName = makeName.Trim();
    }

     private VehicleMake() { }
}
