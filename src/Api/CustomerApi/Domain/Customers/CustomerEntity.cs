using CustomerApi.Common.Models;
using CustomerApi.Common.MongoDbServices;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CustomerApi.Domain.Customers;

[BsonCollection("Customers")]
public class CustomerEntity : BaseEntity
{
    [BsonElement("customer_id")]
    [BsonRepresentation(BsonType.Int32)]
    public int CustomerId { get; set; }
    
    [BsonElement("name")]
    [BsonRepresentation(BsonType.String)]
    public required string  Name { get; set; } 
    
    [BsonElement("address")]
    public Address Address { get; set; }
    
}