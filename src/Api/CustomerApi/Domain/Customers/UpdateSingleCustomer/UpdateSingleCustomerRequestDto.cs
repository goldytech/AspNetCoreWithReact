namespace CustomerApi.Domain.Customers.UpdateSingleCustomer;

public record UpdateSingleCustomerRequestDto
{
    
    public required string Name { get; init; }
    public required string City { get; init; }
}