using Demands.Domain.Entities.Generic;
using System.Linq.Expressions;

namespace Demands.Persistence.Interfaces;

public interface IRepository<TEntity, TId>
    where TEntity: IEntity<TId>
{
    public Task CreateAsync(TEntity entity, CancellationToken ct = default);

    public Task<IReadOnlyCollection<TEntity>> GetAllAsync(CancellationToken ct = default);

    public Task<IReadOnlyCollection<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter, CancellationToken ct = default);

    public Task<TEntity?> GetAsync(TId id, CancellationToken ct = default);

    public Task<IReadOnlyCollection<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter, CancellationToken ct = default);

    public Task RemoveAsync(TId id, CancellationToken ct = default);

    public Task RemoveAsync(TEntity entity, CancellationToken ct = default);

    public Task ReplaceAsync(TEntity entity, CancellationToken ct = default);

    public IQueryable<TEntity> AsQueryable();
}
