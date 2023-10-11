using Demands.Application.Exceptions;
using Demands.Application.Extensions;
using Demands.Domain.Entities;
using Demands.Persistence.Interfaces;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver.Linq;

namespace Demands.Application;

public sealed class ApplicationLogic: IApplicationLogic
{
    private readonly ILogger<ApplicationLogic> _logger;

    private readonly IUnitOfWork _uow;

    public ApplicationLogic(ILogger<ApplicationLogic> logger, IUnitOfWork uow)
    {
        _logger = logger;
        _uow = uow;
    }

    public async Task<Demand?> GetActiveDemandWithUserId(int userId, CancellationToken ct = default)
    {
        IReadOnlyCollection<Demand> queryResult = await _uow.Demands.GetAllAsync(demand => demand.IsActive(), ct);

        switch (queryResult.Count())
        {
            case 0:
                return null;
            case 1:
                return queryResult.First();
            default:
                throw new DuplicateActiveDemandsException();
        }
    }

    public async Task CreateDemandInPersistence(Demand demand, CancellationToken ct = default)
    {
        if (_uow.Demands.AsQueryable().Where(demand => demand.IsActive()).Count() > 1)
        {
            throw new DuplicateActiveDemandsException();
        }
        
        await _uow.Demands.CreateAsync(demand, ct);
    }

    public async Task<Demand> UpdateCustomerGeoAsync(ObjectId demandId, GeoPosition position, CancellationToken ct = default)
    {
        Demand updatedDemand = await _uow.Demands.UpdateDemandCustomerCurrentGeo(demandId, position, ct);
        return updatedDemand;
    }

    public async Task<Demand> UpdateDemandStage(ObjectId demandId, DemandStage stage, CancellationToken ct = default)
    {
        Demand updatedDemand = await _uow.Demands.UpdateDemandStage(demandId, stage, ct);
        return updatedDemand;
    }
}
