using System.Text;
using System.Text.Json;
using BackendForFrontend.Dto;
using Dapr.Client;

namespace BackendForFrontend;

public interface ITokenService
{
    Task<string?> GetJwtTokenForApi(string audience, DaprClient daprClient);
}

public class TokenService : ITokenService
{
    private readonly ILogger<TokenService> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    

    public TokenService(ILogger<TokenService> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        
    }

    public async Task<string?> GetJwtTokenForApi(string audience, DaprClient daprClient)
    {
        var token = await daprClient.GetStateAsync<string>("statestore", $"token-{audience}");
        if (!string.IsNullOrEmpty(token))
        {
            _logger.LogInformation(1, "Token found in state store");
            return token;
        }
        _logger.LogInformation(2, "Token not found in state store, requesting new token");
        var client = _httpClientFactory.CreateClient();
        var tokenModel = new TokenModel("admin","123",audience);
        // Request a token from the Auth service
        var response = await client.PostAsJsonAsync("http://localhost:5135/Auth/token", tokenModel);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogInformation("Error getting token from auth service {StatusCode} {ErrorMessage}", response.StatusCode, response.ReasonPhrase);
            return null;
        }

        var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();
        _logger.LogInformation("Got token from auth service {Token}", tokenResponse);
        
        _logger.LogInformation("Access Token retrieved from auth service {AccessToken}", tokenResponse.AccessToken);
        var metaData = new Dictionary<string, string>
        {
            ["ttlInSeconds"] = "3600" // This should be similar or less than token expiration time
        };
        await daprClient.SaveStateAsync("statestore", $"token-{audience}", 
            tokenResponse?.AccessToken,metadata: metaData);
        _logger.LogInformation(3, "Token saved in state store");
        return tokenResponse?.AccessToken;

    }
}