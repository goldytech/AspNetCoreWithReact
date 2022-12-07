namespace CustomerApi.Domain.Customers.GetAllCustomers;

public record GetAllCustomersResponseDto
{
    public string Id { get; init; }
    public string Name { get; init; }
    public int CustomerId { get; init; }
}