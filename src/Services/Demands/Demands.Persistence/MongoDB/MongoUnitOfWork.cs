using Demands.Domain.Entities;
using Demands.Persistence.Interfaces;
using MongoDB.Driver;

namespace Demands.Persistence.MongoDB;

public class MongoUnitOfWork : IUnitOfWork
{
    public IDemandsRepository Demands { get; init; }

    public MongoUnitOfWork(IMongoDatabase database, string demandsCollectionName)
    {
        Demands = new DemandsMongoRepository(database.GetCollection<Demand>(demandsCollectionName));
    }
}
