namespace FSH.Starter.Blazor.Infrastructure.Connectivity;

/// <summary>
/// Interface for managing SignalR connection hub.
/// Provides real-time connection status and user online/offline tracking.
/// </summary>
public interface IConnectionHubService : IAsyncDisposable
{
    /// <summary>
    /// Event triggered when a user comes online.
    /// </summary>
    event Func<string, Task>? OnUserOnline;

    /// <summary>
    /// Event triggered when a user goes offline.
    /// </summary>
    event Func<string, Task>? OnUserOffline;

    /// <summary>
    /// Gets whether the hub connection is currently active.
    /// </summary>
    bool IsConnected { get; }

    /// <summary>
    /// Gets the current connection state.
    /// </summary>
    ConnectionState ConnectionState { get; }

    /// <summary>
    /// Gets the SignalR connection ID.
    /// </summary>
    string? ConnectionId { get; }

    /// <summary>
    /// Starts the SignalR connection to the connection hub.
    /// </summary>
    Task StartAsync();

    /// <summary>
    /// Stops the SignalR connection.
    /// </summary>
    Task StopAsync();

    /// <summary>
    /// Gets the list of currently online users.
    /// </summary>
    Task<IEnumerable<string>> GetOnlineUsersAsync();

    /// <summary>
    /// Checks if a specific user is currently online.
    /// </summary>
    /// <param name="userId">The user identifier to check.</param>
    Task<bool> IsUserOnlineAsync(string userId);
}

