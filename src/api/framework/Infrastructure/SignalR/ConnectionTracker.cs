using System.Collections.Concurrent;

namespace FSH.Framework.Infrastructure.SignalR;

/// <summary>
/// In-memory implementation of connection tracking for SignalR hubs.
/// Maintains a thread-safe mapping of user IDs to their active connection IDs.
/// </summary>
public class ConnectionTracker : IConnectionTracker
{
    private readonly ConcurrentDictionary<string, HashSet<string>> _connections = new();
    private readonly SemaphoreSlim _lock = new(1, 1);

    /// <inheritdoc/>
    public async Task AddConnectionAsync(string userId, string connectionId)
    {
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(connectionId);

        await _lock.WaitAsync();
        try
        {
            _connections.AddOrUpdate(
                userId,
                new HashSet<string> { connectionId },
                (_, connections) =>
                {
                    connections.Add(connectionId);
                    return connections;
                });
        }
        finally
        {
            _lock.Release();
        }
    }

    /// <inheritdoc/>
    public async Task RemoveConnectionAsync(string userId, string connectionId)
    {
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(connectionId);

        await _lock.WaitAsync();
        try
        {
            if (_connections.TryGetValue(userId, out var connections))
            {
                connections.Remove(connectionId);
                if (connections.Count == 0)
                {
                    _connections.TryRemove(userId, out _);
                }
            }
        }
        finally
        {
            _lock.Release();
        }
    }

    /// <inheritdoc/>
    public Task<IEnumerable<string>> GetConnectionsAsync(string userId)
    {
        ArgumentNullException.ThrowIfNull(userId);

        if (_connections.TryGetValue(userId, out var connections))
        {
            return Task.FromResult<IEnumerable<string>>(connections.ToList());
        }

        return Task.FromResult(Enumerable.Empty<string>());
    }

    /// <inheritdoc/>
    public Task<IEnumerable<string>> GetOnlineUsersAsync()
    {
        return Task.FromResult<IEnumerable<string>>(_connections.Keys.ToList());
    }

    /// <inheritdoc/>
    public Task<bool> IsUserOnlineAsync(string userId)
    {
        ArgumentNullException.ThrowIfNull(userId);

        return Task.FromResult(_connections.ContainsKey(userId));
    }
}
