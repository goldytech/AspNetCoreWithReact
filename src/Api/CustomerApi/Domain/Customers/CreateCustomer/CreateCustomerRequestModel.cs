using CustomerApi.Common.Models;

namespace CustomerApi.Domain.Customers.CreateCustomer;

public class CreateCustomerRequestModel
{
    public required string Name { get; set; }
    public Address Address { get; set; }
}