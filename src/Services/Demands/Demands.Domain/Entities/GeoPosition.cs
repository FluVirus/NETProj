namespace Demands.Domain.Entities;

public struct GeoPosition
{
    public required float Latitude { get; init; }

    public required float Longitude { get; init; }

    public float? Altitude { get; }

    public GeoPosition(float latitude, float longitude, float? altitude) 
    { 
        if (latitude < -90f || latitude > 90f)
        {
            throw new ArgumentOutOfRangeException(paramName: nameof(latitude), message: "Latitude must be in range -90...90");
        }

        if (longitude < -180f || longitude > 180f)
        {
            throw new ArgumentOutOfRangeException(paramName: nameof(longitude), message: "Longitude must be in range -180...180");
        }

        (Latitude, Longitude, Altitude) = (latitude, longitude, altitude);
    } 

    public GeoPosition(float latitude, float longitude) : this(latitude, longitude, null)
    {

    }
}