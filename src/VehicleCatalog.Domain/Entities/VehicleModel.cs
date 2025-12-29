namespace VehicleCatalog.Domain.Entities;

public class VehicleModel
{
    public int MakeId { get; private set; }
    public int ModelId { get; private set; }
    public string ModelName { get; private set; }
    public int ModelYear { get; private set; }

    public VehicleModel(int makeId, int modelId, string modelName, int modelYear)
    {
        if (makeId <= 0)
            throw new ArgumentException("MakeId must be greater than zero", nameof(makeId));
        
        if (modelId <= 0)
            throw new ArgumentException("ModelId must be greater than zero", nameof(modelId));
        
        if (string.IsNullOrWhiteSpace(modelName))
            throw new ArgumentException("ModelName cannot be null or empty", nameof(modelName));

        if (modelYear < 1900 || modelYear > DateTime.Now.Year + 2)
            throw new ArgumentException($"ModelYear must be between 1900 and {DateTime.Now.Year + 2}", nameof(modelYear));

        MakeId = makeId;
        ModelId = modelId;
        ModelName = modelName.Trim();
        ModelYear = modelYear;
    }

    private VehicleModel() { }
}
