using CustomerApi.Common.Models;

namespace CustomerApi.Domain.Customers.GetSingleCustomer;

public class SingleCustomerResponseModel
{
    public string Name { get; set; }
    public Address Address { get; set; }
}