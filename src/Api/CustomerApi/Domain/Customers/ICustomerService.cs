using CustomerApi.Domain.Customers.CreateCustomer;

namespace CustomerApi.Domain.Customers;

public interface ICustomerService
{
    Task<Guid> CreateCustomerAsync(CreateCustomerRequestModel customer);
}