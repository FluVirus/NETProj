using Demands.Domain.Entities.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Demands.Domain.Entities;

public class Demand: IEntity<ObjectId>
{
    [BsonId]
    public ObjectId Id { get; set; }

    public required int CustomerId { get; init; }

    public int? DriverId { get; set; }

    public required DemandStage Stage { get; set; }

    public required DateTime PlacementDateTime { get; init; }

    public required GeoPosition DestinationGeoPosition { get; set; }

    public required GeoPosition CustomerCurrentGeoPosition { get; set; }

    public DateTime? ArrivedDateTime { get; set; }

    public string? ConcurrencyStamp { get; set; }
}
