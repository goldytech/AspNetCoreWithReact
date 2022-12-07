using CustomerApi.Common.Models;

namespace CustomerApi.Domain.Customers.GetSingleCustomer;

public record SingleCustomerResponseDto
{
    public string Name { get; init; }
    public Address Address { get; init; }
}