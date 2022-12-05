using CustomerApi.Common.Entities;
using CustomerApi.Common.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace CustomerApi.Domain.Customers;

public class CustomerEntity : BaseEntity
{
    [BsonElement("customer_id")]
    public int CustomerId { get; set; }
    [BsonElement("name")]
    public required string  Name { get; set; } 
    [BsonElement("address")]
    public Address Address { get; set; }
    
}