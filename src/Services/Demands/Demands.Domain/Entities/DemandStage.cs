namespace Demands.Domain.Entities;

public enum DemandStage: byte
{
    NotRespondedByDriver = 0,
    RespondedByDriver,
    AwaitingPassanger,
    MovingToDestination,
    ArrivedToDestination,
    Terminated,
    Cancelled
}
