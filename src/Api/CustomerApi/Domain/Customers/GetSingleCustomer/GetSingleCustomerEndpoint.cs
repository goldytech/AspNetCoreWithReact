using CustomerApi.Core;

namespace CustomerApi.Domain.Customers.GetSingleCustomer;

public class GetSingleCustomerEndpoint
{
    public static async Task<IResult> GetById(string customerId, ICustomerService  customerService)
    {
        Result<SingleCustomerResponseModel, Exception>? result = null;
        try
        {
            result = await customerService.GetCustomerByIdAsync(customerId);

            return result switch
            {
                { IsSuccess: true, SuccessValue.Name: { } } => TypedResults.Ok(result.SuccessValue),
                {IsSuccess:true,  SuccessValue.Name: null} => TypedResults.NotFound(),
                _ => TypedResults.Empty
            };
        }
        catch (Exception e)
        {
            return TypedResults.Problem(result?.FailureValue.Message);        
        }
    }
}