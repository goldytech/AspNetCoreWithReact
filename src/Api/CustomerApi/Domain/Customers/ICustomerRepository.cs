using System.Security.Authentication;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace CustomerApi.Domain.Customers;

public interface ICustomerRepository
{
    Task<int> CreateAsync(CustomerEntity customer);
}

class CustomerRepository : ICustomerRepository
{
    private readonly ILogger<CustomerRepository> _logger;
    private readonly IMongoCollection<CustomerEntity> _customerCollection;

    public CustomerRepository(ILogger<CustomerRepository> logger)
    {
        _logger = logger;
        var mongoClientSettings = MongoClientSettings.FromConnectionString("mongodb://localhost:27017");
        mongoClientSettings.SslSettings = new SslSettings { EnabledSslProtocols = SslProtocols.Tls12 };
        mongoClientSettings.LinqProvider = LinqProvider.V3;
        _customerCollection = new MongoClient(mongoClientSettings).GetDatabase("EShopDb").GetCollection<CustomerEntity>("Customers");    
    }
    public async Task<int> CreateAsync(CustomerEntity customer)
    {
        try
        {
            await _customerCollection.InsertOneAsync(customer);
            return customer.CustomerId;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while creating customer");
            throw;
        }
       
    }
}