using System.Text.Json.Serialization;

namespace VehicleCatalog.Infrastructure.Models;

public class NHTSAMakeResponse
{
    [JsonPropertyName("Count")]
    public int Count { get; set; }

    [JsonPropertyName("Message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("Results")]
    public List<NHTSAMake> Results { get; set; } = new();
}

public class NHTSAMake
{
    [JsonPropertyName("Make_ID")]
    public int MakeId { get; set; }

    [JsonPropertyName("Make_Name")]
    public string MakeName { get; set; } = string.Empty;
}

public class NHTSAVehicleTypeResponse
{
    [JsonPropertyName("Count")]
    public int Count { get; set; }

    [JsonPropertyName("Message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("Results")]
    public List<NHTSAVehicleType> Results { get; set; } = new();
}

public class NHTSAVehicleType
{
    [JsonPropertyName("VehicleTypeId")]
    public int VehicleTypeId { get; set; }

    [JsonPropertyName("VehicleTypeName")]
    public string VehicleTypeName { get; set; } = string.Empty;
}

public class NHTSAModelResponse
{
    [JsonPropertyName("Count")]
    public int Count { get; set; }

    [JsonPropertyName("Message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("Results")]
    public List<NHTSAModel> Results { get; set; } = new();
}

public class NHTSAModel
{
    [JsonPropertyName("Make_ID")]
    public int MakeId { get; set; }

    [JsonPropertyName("Model_ID")]
    public int ModelId { get; set; }

    [JsonPropertyName("Model_Name")]
    public string ModelName { get; set; } = string.Empty;

    [JsonPropertyName("ModelYear")]
    public int ModelYear { get; set; }
}
