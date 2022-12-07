using CustomerApi.Core;
using CustomerApi.Core.Validation;

namespace CustomerApi.Domain.Customers.CreateCustomer;

public class CreateCustomerEndpoint
{
    public static async Task<IResult> CreateCustomer([Validate]CreateCustomerRequestModel request, ICustomerService  customerService)
    {
        Result<string, Exception>? result = null;
        try
        {
            
            result = await customerService.CreateCustomerAsync(request);
            if (result is { IsSuccess: true })
            {
                return TypedResults.CreatedAtRoute(routeName:"GetById", routeValues: new { customerId = result.SuccessValue }, value: result.SuccessValue);
            }

            return TypedResults.Empty; // for failure

        }
        catch (Exception e)
        {
            return TypedResults.Problem(result?.FailureValue.Message);
        }


        
    }
}