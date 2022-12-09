using MongoDB.Bson.Serialization.Attributes;

namespace CustomerApi.Common.Models;

public class Address
{
    [BsonElement("street")]
    public required string Street { get; set; }
    [BsonElement("city")]
    public required string City { get; set; }
    [BsonElement("state")]
    public required string State { get; set; }
    [BsonElement("zip")]
    public required string Zip { get; set; }
}