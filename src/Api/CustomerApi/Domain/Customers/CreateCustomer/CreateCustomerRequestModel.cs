using CustomerApi.Common.Models;

namespace CustomerApi.Domain.Customers.CreateCustomer;

public class CreateCustomerRequestModel
{
    public required string Name { get; init; }
    public required Address Address { get; init; }

    public CreateCustomerRequestModel(Address address, string name)
    {
        Address = address;
        Name = name;
    }
}