namespace FSH.Framework.Infrastructure.SignalR;

/// <summary>
/// Interface for tracking SignalR user connections.
/// Maintains mapping between user IDs and their active connection IDs.
/// </summary>
public interface IConnectionTracker
{
    /// <summary>
    /// Adds a connection for a user.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="connectionId">The SignalR connection identifier.</param>
    Task AddConnectionAsync(string userId, string connectionId);

    /// <summary>
    /// Removes a connection for a user.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="connectionId">The SignalR connection identifier.</param>
    Task RemoveConnectionAsync(string userId, string connectionId);

    /// <summary>
    /// Gets all connection IDs for a specific user.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <returns>A collection of connection IDs.</returns>
    Task<IEnumerable<string>> GetConnectionsAsync(string userId);

    /// <summary>
    /// Gets all currently online user IDs.
    /// </summary>
    /// <returns>A collection of online user IDs.</returns>
    Task<IEnumerable<string>> GetOnlineUsersAsync();

    /// <summary>
    /// Checks if a user is currently online.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <returns>True if the user has at least one active connection.</returns>
    Task<bool> IsUserOnlineAsync(string userId);
}

