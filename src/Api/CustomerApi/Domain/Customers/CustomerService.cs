using CustomerApi.Common.MongoDbServices;
using CustomerApi.Core;
using CustomerApi.Domain.Customers.CreateCustomer;
using CustomerApi.Domain.Customers.GetSingleCustomer;

namespace CustomerApi.Domain.Customers;

class CustomerService : ICustomerService
{
    private readonly IRepository<CustomerEntity> _customerRepository;

    public CustomerService(IRepository<CustomerEntity> customerRepository)
    {
        _customerRepository = customerRepository;
    }
   
    public async Task<Result<string, Exception>?> CreateCustomerAsync(CreateCustomerRequestModel customer)
    {
        try
        {
            var customerEntity = new CustomerEntity
            {
                CustomerId = 100,
                Name = customer.Name,
                Address = customer.Address,
            
            };

            await _customerRepository.InsertOneAsync(customerEntity);
            return Result<string, Exception>.SucceedWith(customerEntity.Id.ToString());

        }
        catch (Exception e)
        {
            return Result<string, Exception>.FailWith(e);
        }
    }

    public Task<Result<SingleCustomerResponseModel, Exception>> GetCustomerByIdAsync(string id)
    {
         var customer =  _customerRepository.FilterBy(x => x.Id.Equals(id),c => 
             new SingleCustomerResponseModel{Name = c.Name, Address = c.Address});
         return Task.FromResult(Result<SingleCustomerResponseModel, Exception>.SucceedWith(customer.FirstOrDefault() ?? new SingleCustomerResponseModel()));
    }
}