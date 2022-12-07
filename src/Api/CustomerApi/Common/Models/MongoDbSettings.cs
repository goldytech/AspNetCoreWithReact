namespace CustomerApi.Common.Models;

public interface IMongoDbSettings
{
    string DatabaseName { get; set; }
    string ConnectionString { get; set; }
}
public class MongoDbSettings : IMongoDbSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}