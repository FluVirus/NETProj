using MongoDB.Bson.Serialization.Attributes;

namespace Demands.Domain.Entities.Generic;

public interface IEntity<TId>
{
    [BsonId]
    public TId? Id { get; set; }
}
