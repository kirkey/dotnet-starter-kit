using Microsoft.AspNetCore.Authorization;

namespace FSH.Framework.Infrastructure.SignalR;

/// <summary>
/// SignalR hub for tracking real-time connection status.
/// Handles user online/offline status and connection tracking.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ConnectionHub"/> class.
/// </remarks>
/// <param name="connectionTracker">The connection tracker service.</param>
/// <param name="logger">The logger instance.</param>
[Authorize]
public class ConnectionHub(
    IConnectionTracker connectionTracker,
    ILogger<ConnectionHub> logger) : Hub
{
    /// <summary>
    /// Called when a client connects to the hub.
    /// Tracks the connection and notifies other users of online status.
    /// </summary>
    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        if (!string.IsNullOrEmpty(userId))
        {
            await connectionTracker.AddConnectionAsync(userId, Context.ConnectionId);

            // Notify all clients about user online status
            await Clients.Others.SendAsync("UserOnline", userId);

            logger.LogInformation("User {UserId} connected with connection {ConnectionId}", userId, Context.ConnectionId);
        }

        await base.OnConnectedAsync();
    }

    /// <summary>
    /// Called when a client disconnects from the hub.
    /// Removes the connection and notifies other users if the user has no more active connections.
    /// </summary>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.UserIdentifier;
        if (!string.IsNullOrEmpty(userId))
        {
            await connectionTracker.RemoveConnectionAsync(userId, Context.ConnectionId);

            // Check if user still has other active connections
            var isStillOnline = await connectionTracker.IsUserOnlineAsync(userId);
            if (!isStillOnline)
            {
                // Notify all clients that user is offline
                await Clients.Others.SendAsync("UserOffline", userId);
            }

            logger.LogInformation("User {UserId} disconnected from connection {ConnectionId}", userId, Context.ConnectionId);
        }

        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// Gets the list of currently online users.
    /// </summary>
    /// <returns>A list of online user IDs.</returns>
    public async Task<IEnumerable<string>> GetOnlineUsers()
    {
        return await connectionTracker.GetOnlineUsersAsync();
    }

    /// <summary>
    /// Checks if a specific user is currently online.
    /// </summary>
    /// <param name="userId">The user identifier to check.</param>
    /// <returns>True if the user is online, otherwise false.</returns>
    public async Task<bool> IsUserOnline(string userId)
    {
        ArgumentException.ThrowIfNullOrEmpty(userId);
        return await connectionTracker.IsUserOnlineAsync(userId);
    }
}

