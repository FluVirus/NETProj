using Demands.Domain.Entities;
using MongoDB.Bson;

namespace Demands.Application;

public interface IApplicationLogic
{
    public Task<Demand?> GetActiveDemandWithUserId(int userId, CancellationToken ct = default);

    public Task CreateDemandInPersistence(Demand demand, CancellationToken ct = default);

    public Task<Demand> UpdateCustomerGeoAsync(ObjectId demandId, GeoPosition position, CancellationToken ct = default);

    public Task<Demand> UpdateDemandStage(ObjectId demandId, DemandStage stage, CancellationToken ct = default);
}
