using CustomerApi.Core.Validation;

namespace CustomerApi.Domain.Customers.CreateCustomer;

public class CreateCustomerEndpoint
{
    public static async Task<IResult> CreateCustomer([Validate]CreateCustomerRequestModel request, ICustomerService  customerService)
    {
        var customerId = await customerService.CreateCustomerAsync(request);
        return TypedResults.Ok(customerId);
    }
}