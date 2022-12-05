using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CustomerApi.Common.Entities;

public abstract class BaseEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [BsonElement("updated_at")]
    public DateTime? UpdatedAt { get; set; }
    
    [BsonElement("created_by")]
    public string CreatedBy { get; set; }
    
    [BsonElement("updated_by")]
    public string? UpdatedBy { get; set; }
}