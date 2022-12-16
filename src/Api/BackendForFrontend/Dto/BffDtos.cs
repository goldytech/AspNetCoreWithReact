namespace BackendForFrontend.Dto;

public record Customer(string Id, string Name, string Email);

public record GetAllCustomersResponseDto
{
    public string Id { get; init; }
    public string Name { get; init; }
    public int CustomerId { get; init; }
}
public record TokenModel(string ClientId, string ClientSecret, string Audience);

public record TokenResponse(string AccessToken);

public record CreateCustomerRequestDto
{
    public required string Name { get; init; }
    
    public required int CustomerId { get; init; }
    public required Address Address { get; init; }
   
}

public class Address
{
 
    public required string Street { get; set; }
 
    public required string City { get; set; }
 
    public required string State { get; set; }
 
    public required string Zip { get; set; }
}

public record SingleCustomerResponseDto
{
    public string Name { get; init; }
    public Address Address { get; init; }
}