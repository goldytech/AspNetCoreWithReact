using CustomerApi.Common.Models;

namespace CustomerApi.Domain.Customers.CreateCustomer;

public record CreateCustomerRequestDto
{
    public required string Name { get; init; }
    
    public required int CustomerId { get; init; }
    public required Address Address { get; init; }

   
}