using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace SignalR.Notifications.Controllers;

public class NotificationController : ControllerBase
{
    private readonly NotificationHub _notificationHub;

    public NotificationController(NotificationHub notificationHub)
    {
        _notificationHub = notificationHub;
    }
    
    
    [HttpPost]
    [Route("api/notification")]
    public async Task<IActionResult> Post([FromBody] Notification notification)
    {
      await _notificationHub.UpdateUI(notification);
      return Ok();
    }
   
}