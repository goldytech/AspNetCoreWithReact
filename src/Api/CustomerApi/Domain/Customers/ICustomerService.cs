using CustomerApi.Core;
using CustomerApi.Domain.Customers.CreateCustomer;
using CustomerApi.Domain.Customers.GetSingleCustomer;

namespace CustomerApi.Domain.Customers;

public interface ICustomerService
{
    Task<Result<string, Exception>?> CreateCustomerAsync(CreateCustomerRequestModel customer);
    Task<Result<SingleCustomerResponseModel, Exception>> GetCustomerByIdAsync(string id);
}