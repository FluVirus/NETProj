using Demands.Domain.Entities.Generic;
using Demands.Persistence.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Demands.Persistence.MongoDB;

public class MongoRepository<T> : IRepository<T, ObjectId>
    where T : IEntity<ObjectId>
{
    protected readonly IMongoCollection<T> _collection;

    protected readonly FilterDefinitionBuilder<T> _filterBuilder = Builders<T>.Filter;

    protected readonly UpdateDefinitionBuilder<T> _updateBuilder = Builders<T>.Update;

    private readonly InsertOneOptions _createAsyncOptions = new InsertOneOptions
    {

    };

    private readonly ReplaceOptions _replaceAsyncOptions = new ReplaceOptions
    {
        IsUpsert = false
    };

    public MongoRepository(IMongoCollection<T> collection)
    {
        _collection = collection;
    }

    public async Task CreateAsync(T entity, CancellationToken ct = default)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        await _collection.InsertOneAsync(entity, _createAsyncOptions ,ct);
    }

    public async Task<IReadOnlyCollection<T>> GetAllAsync(CancellationToken ct = default)
    {
        return await _collection.Find(_filterBuilder.Empty).ToListAsync(ct);
    }

    public async Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter, CancellationToken ct = default)
    {
        return await _collection.Find(filter).ToListAsync(ct);
    }

    public async Task<T?> GetAsync(ObjectId id, CancellationToken ct = default)
    {
        FilterDefinition<T> filter = _filterBuilder.Eq(entity => entity.Id, id);
        return await _collection.Find(filter).FirstOrDefaultAsync(ct);
    }

    public async Task<IReadOnlyCollection<T>> GetAsync(Expression<Func<T, bool>> filter, CancellationToken ct = default)
    {
        return await _collection.Find(filter).ToListAsync(ct);
    }

    public async Task RemoveAsync(ObjectId id, CancellationToken ct = default)
    {
        if (id == default(ObjectId))
        {
            throw new ArgumentNullException(nameof(id));
        }

        FilterDefinition<T> filter = _filterBuilder.Eq(entity => entity.Id, id);
        await _collection.DeleteOneAsync(filter, ct);
    }

    public async Task RemoveAsync(T entity, CancellationToken ct = default)
    {
        await RemoveAsync(entity.Id, ct);
    }

    public async Task ReplaceAsync(T entity, CancellationToken ct = default)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        if (entity.Id == default(ObjectId))
        {
            throw new ArgumentException(message: "Try to update not created document", paramName: nameof(entity));
        }
        
        FilterDefinition<T> filter = _filterBuilder.Eq(entity => entity.Id, entity.Id);

        await _collection.ReplaceOneAsync(filter, entity, _replaceAsyncOptions, ct);
    }

    public IQueryable<T> AsQueryable() => _collection.AsQueryable();
}
