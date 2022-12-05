using CustomerApi.Core;
using CustomerApi.Domain.Customers.CreateCustomer;

namespace CustomerApi.Domain.Customers;

class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
   
    public async Task<Result<int, Exception>> CreateCustomerAsync(CreateCustomerRequestModel customer)
    {
        try
        {
            var customerEntity = new CustomerEntity
            {
                CustomerId = 100,
                Id = Guid.NewGuid().ToString(),
                Name = customer.Name,
                Address = customer.Address,
            
            };

            var customerId = await _customerRepository.CreateAsync(customerEntity);
            return Result<int, Exception>.SucceedWith(customerId);

        }
        catch (Exception e)
        {
            return Result<int, Exception>.FailWith(e);
        }
    }
}