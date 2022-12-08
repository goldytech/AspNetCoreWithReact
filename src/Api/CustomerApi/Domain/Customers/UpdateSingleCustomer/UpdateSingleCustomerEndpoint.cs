namespace CustomerApi.Domain.Customers.UpdateSingleCustomer;

public class UpdateSingleCustomerEndpoint
{
    public static async Task<IResult> UpdateCustomer(string customerId, UpdateSingleCustomerRequestDto request,
        ICustomerService customerService)
    {
        var result = await customerService.UpdateCustomerAsync(customerId, request);
        return result.SuccessValue switch
        {
            null => TypedResults.NotFound(customerId),
            true => TypedResults.NoContent(),
            _ => result.IsSuccess is false ? TypedResults.Problem(result.FailureValue.Message) : TypedResults.Empty
        };
    }
}