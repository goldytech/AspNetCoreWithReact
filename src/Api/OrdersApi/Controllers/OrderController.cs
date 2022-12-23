using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;

namespace OrdersApi.Controllers;

[ApiController]
[Route("[controller]")]

public class OrderController : ControllerBase
{
    private readonly ILogger<OrderController> _logger;
    private readonly DaprClient _daprClient;
    private const string PubSubName = "redis-pubsub";

    public OrderController(ILogger<OrderController> logger, DaprClient daprClient)
    {
        _logger = logger;
        _daprClient = daprClient;
    }
   
    // Submit Order
    [HttpPost]
    [Route("submit")]
    public IActionResult SubmitOrder([FromBody] Order order)
    {
        // Submit Order
        _logger.LogInformation("Order Submitted for {OrderId}", order.Id);
        _daprClient.PublishEventAsync(PubSubName, "ordersubmitted", order);
        return Ok();
    }
    
    // Handle  Order Submitted Event
    [Topic(PubSubName, "ordersubmitted")]
    [HttpPost]
    [Route("handleordersubmitted")]
    public IActionResult HandleSubmitOrder([FromBody] Order order)
    {
        // Handle Order
        _logger.LogInformation("Order Handled for {OrderId}", order.Id);
        return Ok(); // Always return ok for event handlers , if there is any error in processing the event, it will be retried by Dapr
    }
}

public record Order(int Id, string Name, IEnumerable<OrderDetails> LineItems);
public record OrderDetails(int Id, string Name, string Description);