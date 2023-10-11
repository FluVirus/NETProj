namespace Demands.Persistence.Interfaces;

public interface IUnitOfWork
{
    public IDemandsRepository Demands { get; init; }
}
