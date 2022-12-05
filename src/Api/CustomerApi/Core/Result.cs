namespace CustomerApi.Core;

public class Result<TSuccess, TFailure>
{
    private Result(bool isSuccess)
    {
        IsSuccess = isSuccess;
    }

    public TSuccess SuccessValue { get; init; }
    public TFailure FailureValue { get; init; }
    public bool IsSuccess { get; }

    public static Result<TSuccess, TFailure> SucceedWith(TSuccess value)
    {
        return new(true)
        {
            SuccessValue = value
        };
    }

    public static Result<TSuccess, TFailure> FailWith(TFailure value)
    {
        return new(false)
        {
            FailureValue = value
        };
    }
}