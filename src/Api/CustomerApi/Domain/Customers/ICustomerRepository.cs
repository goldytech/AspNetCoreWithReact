// using System.Security.Authentication;
// using CustomerApi.Common.MongoDbServices;
// using MongoDB.Bson;
// using MongoDB.Driver;
// using MongoDB.Driver.Linq;
//
// namespace CustomerApi.Domain.Customers;
//
// public interface ICustomerRepository 
//
// {
//     
//     Task<int> CreateAsync(CustomerEntity customer);
//     Task<CustomerEntity> GetAsync(string id);
// }
//
// public class CustomerRepository : ICustomerRepository
// {
//     private readonly ILogger<CustomerRepository> _logger;
//     private readonly IMongoClient _mongoClient;
//    // private readonly IMongoCollection<BsonDocument> _customerCollection;
//     private readonly IMongoCollection<CustomerEntity> _customerCollection;
//     private readonly IRepository<CustomerEntity> _repository;
//     public CustomerRepository(ILogger<CustomerRepository> logger, IMongoClient mongoClient, IRepository<CustomerEntity> repository)
//     {
//         _logger = logger;
//         _mongoClient = mongoClient;
//         _repository = repository;
//         // _customerCollection = _mongoClient.GetDatabase("EShopDb").GetCollection<BsonDocument>("Customers");
//         _customerCollection = _mongoClient.GetDatabase("EShopDb").GetCollection<CustomerEntity>("Customers");
//         
//     }
//     public async Task<int> CreateAsync(CustomerEntity customer)
//     {
//         try
//         {
//             // var customerBsonDocument = new BsonDocument
//             // {
//             //     { "_id", ObjectId.GenerateNewId() }, { "name", customer.Name }, { "address.street", customer.Address.Street },
//             //     { "address.city", customer.Address.City }, { "address.state", customer.Address.State },
//             //     { "address.zip", customer.Address.Zip },
//             //     { "customer_id", customer.CustomerId }
//             // };
//             // await _customerCollection.InsertOneAsync(customerBsonDocument);
//            
//             await _repository.CreateAsync(customer);
//             return customer.CustomerId; 
//
//         }
//         catch (Exception e)
//         {
//             _logger.LogError(e, "Error while creating customer");
//             throw;
//         }
//
//     }
//
//     public async Task<CustomerEntity> GetAsync(string id)
//     {
//         try
//         {
//             var customer = await _repository.GetByIdAsync(id);
//             return customer;
//         }
//         catch (Exception e)
//         {
//             _logger.LogError(e, "Error while getting customer");
//             throw;
//         }
//         
//         
//     }
// }