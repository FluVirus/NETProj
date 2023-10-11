using Demands.Domain.Entities;
using Demands.Persistence.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Demands.Persistence.MongoDB;

public class DemandsMongoRepository : MongoRepository<Demand>, IDemandsRepository
{
    public DemandsMongoRepository(IMongoCollection<Demand> collection) : base(collection)
    {

    }

    public async Task<Demand> UpdateDemandCustomerCurrentGeo(ObjectId demandId, GeoPosition geo, CancellationToken ct = default)
    {
        FilterDefinition<Demand> findFilter = _filterBuilder.Eq(demand => demand.Id, demandId);
        UpdateDefinition<Demand> updateDefinition = _updateBuilder
            .Set(demand => demand.CustomerCurrentGeoPosition, geo)
            .Set(demand => demand.ConcurrencyStamp, Guid.NewGuid().ToString());

        Demand result = await _collection.FindOneAndUpdateAsync(findFilter, updateDefinition, options: null, ct);

        return result;
    }

    public async Task<Demand> UpdateDemandStage(ObjectId demandId, DemandStage stage, CancellationToken ct = default)
    {
        FilterDefinition<Demand> findFilter = _filterBuilder.Eq(demand => demand.Id, demandId);
        UpdateDefinition<Demand> updateDefinition = _updateBuilder
            .Set(demand => demand.Stage, stage)
            .Set(demand => demand.ConcurrencyStamp, Guid.NewGuid().ToString());

        Demand result = await _collection.FindOneAndUpdateAsync(findFilter, updateDefinition, options: null, ct);

        return result;
    }
}
