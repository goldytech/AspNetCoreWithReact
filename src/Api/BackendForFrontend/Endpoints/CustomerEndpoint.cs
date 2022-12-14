using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using BackendForFrontend.Dto;
using Dapr.Client;

namespace BackendForFrontend.Endpoints;

public class CustomerEndpoint
{
    private readonly ILogger<CustomerEndpoint> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly DaprClient _daprClient;
    private static readonly string[] Emails = new[]
    {
        "joe@gmail.com", "john@outlook.com", "marry@hotmail.com"
    };
    public CustomerEndpoint(ILogger<CustomerEndpoint> logger,IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _daprClient = new DaprClientBuilder().Build();
    }
    public  async Task<IResult> GetAll()
    {
        
        var request = _daprClient.CreateInvokeMethodRequest(HttpMethod.Get, "customers-api",
            "api/v1/customers"); 
        request.Headers.Authorization = new AuthenticationHeaderValue("bearer", 
            await GetJwtTokenForApi("customers-api"));
        
        var customers = await _daprClient.InvokeMethodAsync<IEnumerable<GetAllCustomersResponseDto>>
            (request);

        var customersList = new List<Customer>();
        foreach (var dto in customers)
        {
            customersList.Add(new Customer(dto.Id, dto.Name, Emails[Random.Shared.Next(Emails.Length)]));
        }
        return TypedResults.Ok(customersList);
    }

    private async Task<string?> GetJwtTokenForApi(string audience)
    {
        var token = await _daprClient.GetStateAsync<string>("statestore", $"token-{audience}");
        if (!string.IsNullOrEmpty(token))
        {
            _logger.LogInformation(1, "Token found in state store");
            return token;
        }
        _logger.LogInformation(2, "Token not found in state store, requesting new token");
        var client = _httpClientFactory.CreateClient();
        var tokenModel = new TokenModel("admin","123",audience);
        var content = new StringContent(JsonSerializer.Serialize(tokenModel), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("http://localhost:5135/Auth/login2", content);
        if (!response.IsSuccessStatusCode)
        {
            return token;
        }

        var tokenResponse = await response.Content.ReadAsStringAsync();
        var tokenResponseModel = JsonSerializer.Deserialize<TokenResponse>(tokenResponse);
        await _daprClient.SaveStateAsync("statestore", $"token-{audience}", tokenResponseModel?.AccessToken);
        _logger.LogInformation(3, "Token saved in state store");
        return tokenResponseModel?.AccessToken;

    }
}