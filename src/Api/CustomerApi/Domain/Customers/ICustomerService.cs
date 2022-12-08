using CustomerApi.Core;
using CustomerApi.Domain.Customers.CreateCustomer;
using CustomerApi.Domain.Customers.GetAllCustomers;
using CustomerApi.Domain.Customers.GetSingleCustomer;
using CustomerApi.Domain.Customers.UpdateSingleCustomer;

namespace CustomerApi.Domain.Customers;

public interface ICustomerService
{
    Task<Result<string, Exception>?> CreateCustomerAsync(CreateCustomerRequestDto customer);
    Task<Result<SingleCustomerResponseDto, Exception>> GetCustomerByIdAsync(string id);
    Task<Result<IEnumerable<GetAllCustomersResponseDto>,Exception>> GetAllCustomersAsync();
    Task<Result<bool?,Exception>> UpdateCustomerAsync (string id, UpdateSingleCustomerRequestDto updateSingleCustomerRequestDto);
}