using Dapr.Client;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDaprClient();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/order/submit", async ([FromBody]Order order, [FromServices]DaprClient daprClient) =>
{
    await Task.Delay(1000);
    await daprClient.PublishEventAsync("pubsub-rabbitmq", "order", order);
});

app.Run();

public record Order(int Id, string Name, IEnumerable<OrderDetails> LineItems);
public record OrderDetails(int Id, string Name, string Description);