using Demands.Application;
using Demands.Application.Exceptions;
using Demands.Domain.Entities;
using Demands.WebApi.Constants;
using Demands.WebApi.Extensions;
using Demands.WebApi.Models;
using Demands.WebApi.ViewModels;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Bson;

namespace Demands.WebApi.Hubs;

public class DemandsHub: Hub
{
    private readonly ILogger<DemandsHub> _logger;

    private readonly IApplicationLogic _logic;

    public DemandsHub(ILogger<DemandsHub> logger, IApplicationLogic logic)
    {
        _logger = logger;
        _logic = logic;
    }

    #region HubPublicMethods

    [HubMethodName("geo")]
    public async Task UpdateGeoAsync(GeoPosition position)
    {
        Demand? demand = Context.Items[DemandsSessionKeys.DemandKey] as Demand;

        if (demand is null)
        {
            throw new ProtocolException("[DemandsHub, UpdateGeoAsync] Cannot update geo before demand created");
        }

        Demand updatedDemand = await _logic.UpdateCustomerGeoAsync(demand.Id, position);

        Context.Items[DemandsSessionKeys.DemandKey] = updatedDemand;
    }

    [HubMethodName("cancel")]
    public async Task CancelDemandAsync()
    {
        Demand? demand = Context.Items[DemandsSessionKeys.DemandKey] as Demand;

        if (demand is null)
        {
            throw new ProtocolException("[DemandsHub, CancelDemandAsync] Cannot cancel demand before demand created");
        }

        if (demand.Stage == DemandStage.NotRespondedByDriver || demand.Stage == DemandStage.RespondedByDriver || demand.Stage == DemandStage.AwaitingPassanger)
        {
            throw new ProtocolException("[DemandsHub, CancelDemandAsync] Cannot cancel demand after passanger sit in car");
        }

        Demand updatedDemand = await _logic.UpdateDemandStage(demand.Id, DemandStage.Cancelled);

        Context.Items.Remove(DemandsSessionKeys.DemandKey);
    }

    [HubMethodName("create")]
    public async Task CreateDemandAsync(CreateDemandViewModel createDemand)
    {
        Demand demand = new Demand
        {
            CustomerId = int.Parse(Context.UserIdentifier!),
            PlacementDateTime = createDemand.PlacementDateTime,
            CustomerCurrentGeoPosition = createDemand.InitialGeoPoition,
            DestinationGeoPosition = createDemand.DestinationGeoPosition,
            Stage = DemandStage.NotRespondedByDriver,
            ConcurrencyStamp = Guid.NewGuid().ToString(),
        };
        
        await _logic.CreateDemandInPersistence(demand);
    }

    #endregion

    #region HubInternalMethods

    //From Driver request
    internal async Task DemandConfirmedByDriver(int DriverId, ObjectId DemandId /*TODO: Driver data*/)
    {
        //Is it correct here?
        Demand? demand = Context.Items[DemandsSessionKeys.DemandKey] as Demand;

        if (demand is null)
        {
            throw new ProtocolException("[DemandsHub, DemandConfirmedByDriver] Cannot confirm non existing demand");
        }

        if (demand.Stage != DemandStage.NotRespondedByDriver)
        {
            throw new ProtocolException("[DemandsHub, DemandConfirmedByDriver] Cannot confirm demand that needs no confirmation");
        }

        await Context.Items.DeletePushActionFromContext(DemandsSessionKeys.OverviewContext);

        await Clients.User(Context.UserIdentifier!).SendAsync(DemandsMethodNames.GetConfirmationByDriver /*TODO: Driver data*/);

        PushMethodContext carLocationContext = new(GetCarCurrentLocation);
        Context.Items[DemandsSessionKeys.GetCarLocationContext] = carLocationContext;
        carLocationContext.Start();
    }

    //From Driver request
    internal Task ConfirmSitDownAsync()
    {
        //TODO: data transfer
        throw new NotImplementedException();
    }

    //From Driver or System request
    internal Task DriverAwaitsCustomerAsync()
    {
        //TODO: data transfer
        throw new NotImplementedException();
    }

    //From Driver or System request
    internal Task ArrivedToDestinationAsync()
    {
        //TODO: data transfer
        throw new NotImplementedException();
    }

    #endregion

    #region HubLifetimeEvents
    public override async Task OnConnectedAsync()
    {
        if (string.IsNullOrEmpty(Context.UserIdentifier))
        {
            throw new ArgumentNullException(paramName: nameof(Context.UserIdentifier), message: "[DemandsHub, OnConnectedAsync] User ID is not set in UserClaims but must be");
        }

        int userId = int.Parse(Context.UserIdentifier!);
        Demand? demand = await _logic.GetActiveDemandWithUserId(userId);

        Context.Items[DemandsSessionKeys.DemandKey] = demand;

        if (demand is null)
        {
            PushMethodContext overviewContext = new(GetOverviewForUserAsync);
            Context.Items[DemandsSessionKeys.OverviewContext] = overviewContext;
            overviewContext.Start();
            return;
        }

        switch (demand.Stage)
        {
            case DemandStage.NotRespondedByDriver:
                break;

            case DemandStage.RespondedByDriver:
            case DemandStage.AwaitingPassanger:
            case DemandStage.MovingToDestination:
            case DemandStage.ArrivedToDestination:
                PushMethodContext carLocationContext = new(GetCarCurrentLocation);
                Context.Items[DemandsSessionKeys.GetCarLocationContext] = carLocationContext;
                carLocationContext.Start();
                break;
        }
    }

    public async override Task OnDisconnectedAsync(Exception? exception)
    {
        Demand? demand = Context.Items[DemandsSessionKeys.DemandKey] as Demand;

        if (demand is null)
        {
            await Context.Items.DeletePushActionFromContext(DemandsSessionKeys.OverviewContext);
            return;
        }

        switch (demand.Stage)
        {
            case DemandStage.NotRespondedByDriver:
                break;

            case DemandStage.RespondedByDriver:
            case DemandStage.AwaitingPassanger: 
            case DemandStage.MovingToDestination:
            case DemandStage.ArrivedToDestination:
                await Context.Items.DeletePushActionFromContext(DemandsSessionKeys.GetCarLocationContext);
                break;
        }

        if (exception is not null)
        {
            throw exception;
        }
    }
    #endregion

    #region ServerPushingMethods

    private async Task GetOverviewForUserAsync(CancellationToken cancellationToken)
    {
        if (Context.UserIdentifier is null)
        {
            throw new ArgumentNullException(paramName: nameof(Context.UserIdentifier), message: "[DemandsHub, GetOverviewForUser] User ID is not set in UserClaims but must be");
        }

        while (!cancellationToken.IsCancellationRequested)
        {
            //TODO: _driverService (or disco ). GetInfo()
            await Clients.User(Context.UserIdentifier).SendAsync(DemandsMethodNames.Overview, /*TODO: array of drivers poitions in the region, costs, time progniostication */ cancellationToken);
            await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
        }
    }

    private async Task GetCarCurrentLocation(CancellationToken cancellationToken)
    {
        if (Context.UserIdentifier is null)
        {
            throw new ArgumentNullException(paramName: nameof(Context.UserIdentifier), message: "[DemandsHub, GetOverviewForUser] User ID is not set in UserClaims but must be");
        }

        while (!cancellationToken.IsCancellationRequested) 
        {
            //TODO: _driverService.GetCurrentLocation(Driver.)
            await Clients.User(Context.UserIdentifier).SendAsync(DemandsMethodNames.UpdateCarGeoLocation, /*TODO: location of current car, prognost. time*/ cancellationToken);
            await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
        }
    }

    #endregion
}
