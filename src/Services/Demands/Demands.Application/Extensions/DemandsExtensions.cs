using Demands.Domain.Entities;

namespace Demands.Application.Extensions;

public static class DemandsExtensions
{
    public static bool IsActive(this Demand demand) => demand.Stage != DemandStage.Terminated && demand.Stage != DemandStage.Cancelled;
}
