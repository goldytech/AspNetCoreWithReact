using CustomerApi.Core;

namespace CustomerApi.Domain.Customers.GetAllCustomers;

public class GetAllCustomersEndpoint
{
    public static async Task<IResult> GetAll(ICustomerService customerService)
    {
        var result = await customerService.GetAllCustomersAsync();

        return result.IsSuccess
            ? TypedResults.Ok(result.SuccessValue)
            : TypedResults.Problem(result.FailureValue.Message);
    }
}