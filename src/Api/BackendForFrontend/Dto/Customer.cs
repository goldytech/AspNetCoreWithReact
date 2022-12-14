namespace BackendForFrontend.Dto;

public record Customer(string Id, string Name, string Email);

public record GetAllCustomersResponseDto
{
    public string Id { get; init; }
    public string Name { get; init; }
    public int CustomerId { get; init; }
}
public record TokenModel(string Username, string Password, string Audience);

public record TokenResponse(string? AccessToken);