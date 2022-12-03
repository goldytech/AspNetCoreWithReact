using CustomerApi.Domain.Customers.CreateCustomer;

namespace CustomerApi.Domain.Customers;

class CustomerService : ICustomerService
{
    public Task<Guid> CreateCustomerAsync(CreateCustomerRequestModel customer)
    {
        return Task.FromResult(Guid.NewGuid());
    }
}