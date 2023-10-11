using Demands.Domain.Entities;
using System.Text.Json.Serialization;

namespace Demands.WebApi.ViewModels;

public class CreateDemandViewModel
{
    [JsonPropertyName("p")]
    public required DateTime PlacementDateTime { get; init; }

    [JsonPropertyName("i")]
    public required GeoPosition InitialGeoPoition { get; init; }

    [JsonPropertyName("d")]
    public required GeoPosition DestinationGeoPosition { get; init; }
}
