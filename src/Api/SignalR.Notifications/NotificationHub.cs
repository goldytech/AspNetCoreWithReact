using Microsoft.AspNetCore.SignalR;

namespace SignalR.Notifications;

public class NotificationHub : Hub

{
    public async Task UpdateUI(Notification notification)
    {
        var groupName = $"fe-{notification.CaseId}";
        // Send the notification to the group.
        await Clients.Group(groupName).SendAsync("RefreshUI", notification);
        
    }
    
    public async Task JoinGroup(string groupName)
    {
        var groupWithPrefix = $"fe-{groupName}";
        // Create the group if it doesn't exist.
        // This is a no-op if the group already exists.
        await Groups.AddToGroupAsync(Context.ConnectionId, groupWithPrefix);
    }

    public override Task OnConnectedAsync()
    {
        // TODO - Add logging with details of Context.ConnectionId
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        return base.OnDisconnectedAsync(exception); // TODO - Add logging with details of Context.ConnectionId
    }
}

public class Notification
{
    public string Message { get; set; }
    public string CaseId { get; set; }
}