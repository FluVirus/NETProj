using Demands.Persistence.Configuration;
using Demands.Persistence.Interfaces;
using Demands.Persistence.MongoDB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Demands.Persistence;

public static class ConfigureServices
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        MongoDbConfiguration mongoDbConfiguration = configuration.GetSection("MongoDbConfiguration").Get<MongoDbConfiguration>();
        MongoClient client = new MongoClient(connectionString: mongoDbConfiguration.ConnectionString);
        MongoUnitOfWork uow = new MongoUnitOfWork(client.GetDatabase(mongoDbConfiguration.DemandsDatabaseName), mongoDbConfiguration.DemandsCollectionName);

        services.AddSingleton<IMongoClient>(client);
        services.AddSingleton<IUnitOfWork>(uow);

        return services;
    }
}
