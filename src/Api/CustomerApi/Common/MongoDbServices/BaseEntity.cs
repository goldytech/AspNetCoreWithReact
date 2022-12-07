using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CustomerApi.Common.MongoDbServices;

public interface  IBaseEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
   public ObjectId Id { get; set; }
    
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; }
    
    [BsonElement("updated_at")]
    public DateTime? UpdatedAt { get; set; } 
    
    [BsonElement("created_by")]
    public string CreatedBy { get; set; }
    
    [BsonElement("updated_by")]
    public string? UpdatedBy { get; set; }
}

public abstract class BaseEntity : IBaseEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public ObjectId Id { get; set; }
    
    
    [BsonElement("created_at")]
    public DateTime CreatedAt  => Id.CreationTime;
    
    [BsonElement("updated_at")]
    public DateTime? UpdatedAt { get; set; } 
    
    [BsonElement("created_by")]
    public string CreatedBy { get; set; }
    
    [BsonElement("updated_by")]
    public string? UpdatedBy { get; set; }
}