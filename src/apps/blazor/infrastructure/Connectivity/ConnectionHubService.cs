using Microsoft.AspNetCore.SignalR.Client;

namespace FSH.Starter.Blazor.Infrastructure.Connectivity;

/// <summary>
/// Service for managing SignalR connection hub.
/// Handles real-time connection status and user online/offline tracking.
/// </summary>
public class ConnectionHubService : IConnectionHubService
{
    private readonly IAccessTokenProvider _accessTokenProvider;
    private readonly ILogger<ConnectionHubService> _logger;
    private readonly Notifications.INotificationPublisher _notificationPublisher;
    private readonly Uri _apiBaseUri;
    private HubConnection? _hubConnection;
    private bool _isDisposed;

    public event Func<string, Task>? OnUserOnline;
    public event Func<string, Task>? OnUserOffline;

    public bool IsConnected => _hubConnection?.State == HubConnectionState.Connected;

    public ConnectionState ConnectionState =>
        _hubConnection?.State switch
        {
            HubConnectionState.Connected => ConnectionState.Connected,
            HubConnectionState.Disconnected => ConnectionState.Disconnected,
            _ => ConnectionState.Connecting
        };

    public string? ConnectionId => _hubConnection?.ConnectionId;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConnectionHubService"/> class.
    /// </summary>
    /// <param name="apiBaseUri">The API base URI.</param>
    /// <param name="accessTokenProvider">The access token provider for authentication.</param>
    /// <param name="notificationPublisher">The notification publisher for connection state changes.</param>
    /// <param name="logger">The logger instance.</param>
    public ConnectionHubService(
        Uri apiBaseUri,
        IAccessTokenProvider accessTokenProvider,
        Notifications.INotificationPublisher notificationPublisher,
        ILogger<ConnectionHubService> logger)
    {
        _apiBaseUri = apiBaseUri;
        _accessTokenProvider = accessTokenProvider;
        _notificationPublisher = notificationPublisher;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task StartAsync()
    {
        if (_hubConnection is not null)
        {
            await _hubConnection.DisposeAsync();
        }

        var hubUrl = new Uri(_apiBaseUri, "hubs/connection");

        _hubConnection = new HubConnectionBuilder()
            .WithUrl(hubUrl, options =>
            {
                options.AccessTokenProvider = async () =>
                {
                    var tokenResult = await _accessTokenProvider.RequestAccessToken();
                    if (tokenResult.TryGetToken(out var token))
                    {
                        return token.Value;
                    }
                    return null;
                };
            })
            .WithAutomaticReconnect(new[] { TimeSpan.Zero, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10) })
            .Build();

        // Register event handlers
        _hubConnection.On<string>("UserOnline", async (userId) =>
        {
            if (OnUserOnline != null)
            {
                await OnUserOnline.Invoke(userId);
            }
        });

        _hubConnection.On<string>("UserOffline", async (userId) =>
        {
            if (OnUserOffline != null)
            {
                await OnUserOffline.Invoke(userId);
            }
        });

        _hubConnection.Closed += async (error) =>
        {
            _logger.LogWarning(error, "Connection hub closed");
            await _notificationPublisher.PublishAsync(
                new ConnectionStateChanged(ConnectionState.Disconnected, error?.Message ?? "Connection closed"));
            await Task.Delay(new Random().Next(0, 5) * 1000);
        };

        _hubConnection.Reconnecting += async (error) =>
        {
            _logger.LogWarning(error, "Connection hub reconnecting");
            await _notificationPublisher.PublishAsync(
                new ConnectionStateChanged(ConnectionState.Connecting, "Reconnecting..."));
        };

        _hubConnection.Reconnected += async (connectionId) =>
        {
            _logger.LogInformation("Connection hub reconnected with ID: {ConnectionId}", connectionId);
            await _notificationPublisher.PublishAsync(
                new ConnectionStateChanged(ConnectionState.Connected, connectionId));
        };

        try
        {
            await _hubConnection.StartAsync();
            _logger.LogInformation("Connection hub started successfully");
            await _notificationPublisher.PublishAsync(
                new ConnectionStateChanged(ConnectionState.Connected, _hubConnection.ConnectionId ?? "Connected"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error starting connection hub");
            await _notificationPublisher.PublishAsync(
                new ConnectionStateChanged(ConnectionState.Disconnected, ex.Message));
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task StopAsync()
    {
        if (_hubConnection is not null)
        {
            await _hubConnection.StopAsync();
            _logger.LogInformation("Connection hub stopped");
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<string>> GetOnlineUsersAsync()
    {
        if (_hubConnection is not null && IsConnected)
        {
            try
            {
                return await _hubConnection.InvokeAsync<IEnumerable<string>>("GetOnlineUsers");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting online users");
                return Enumerable.Empty<string>();
            }
        }

        return Enumerable.Empty<string>();
    }

    /// <inheritdoc/>
    public async Task<bool> IsUserOnlineAsync(string userId)
    {
        if (_hubConnection is not null && IsConnected)
        {
            try
            {
                return await _hubConnection.InvokeAsync<bool>("IsUserOnline", userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if user is online");
                return false;
            }
        }

        return false;
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        if (_isDisposed)
        {
            return;
        }

        _isDisposed = true;

        if (_hubConnection is not null)
        {
            try
            {
                await _hubConnection.DisposeAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error disposing connection hub");
            }
        }

        GC.SuppressFinalize(this);
    }
}

