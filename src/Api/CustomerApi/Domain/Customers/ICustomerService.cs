using CustomerApi.Core;
using CustomerApi.Domain.Customers.CreateCustomer;

namespace CustomerApi.Domain.Customers;

public interface ICustomerService
{
    Task<Result<int, Exception>> CreateCustomerAsync(CreateCustomerRequestModel customer);
}