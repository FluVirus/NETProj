using Demands.Domain.Entities;
using MongoDB.Bson;

namespace Demands.Persistence.Interfaces;
public interface IDemandsRepository: IRepository<Demand, ObjectId>
{
    public Task<Demand> UpdateDemandCustomerCurrentGeo(ObjectId demandId, GeoPosition geo, CancellationToken ct = default);

    public Task<Demand> UpdateDemandStage(ObjectId demandId, DemandStage stage, CancellationToken ct = default);
}
