using CustomerApi.Core.Validation;

namespace CustomerApi.Domain.Customers.CreateCustomer;

public class CreateCustomerEndpoint
{
    public static async Task<IResult> CreateCustomer([Validate]CreateCustomerRequestModel request, ICustomerService  customerService)
    {
        var result = await customerService.CreateCustomerAsync(request);
        if (result.IsSuccess)
        {
            return TypedResults.Ok(result.SuccessValue);
        }

        return TypedResults.Problem(result.FailureValue.Message);

    }
}