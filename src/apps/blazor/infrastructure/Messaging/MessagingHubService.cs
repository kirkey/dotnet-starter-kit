using Microsoft.AspNetCore.SignalR.Client;

namespace FSH.Starter.Blazor.Infrastructure.Messaging;

/// <summary>
/// Service for managing SignalR messaging hub connections.
/// Handles real-time messaging, typing indicators, and read receipts.
/// Note: Connection status tracking is handled by ConnectionHubService.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MessagingHubService"/> class.
/// </remarks>
/// <param name="apiBaseUri">The API base URI.</param>
/// <param name="accessTokenProvider">The access token provider for authentication.</param>
/// <param name="logger">The logger instance.</param>
public class MessagingHubService(
    Uri apiBaseUri,
    IAccessTokenProvider accessTokenProvider,
    ILogger<MessagingHubService> logger) : IMessagingHubService
{
    private HubConnection? _hubConnection;
    private bool _isDisposed;

    public event Func<string, object, Task>? OnMessageReceived;
    public event Func<string, string, bool, Task>? OnUserTyping;
    public event Func<string, string, string, Task>? OnMessageRead;

    public bool IsConnected => _hubConnection?.State == HubConnectionState.Connected;


    /// <inheritdoc/>
    public async Task StartAsync()
    {
        if (_hubConnection is not null)
        {
            await _hubConnection.DisposeAsync();
        }

        var hubUrl = new Uri(apiBaseUri, "hubs/messaging");

        _hubConnection = new HubConnectionBuilder()
            .WithUrl(hubUrl, options =>
            {
                options.AccessTokenProvider = async () =>
                {
                    var tokenResult = await accessTokenProvider.RequestAccessToken();
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
        _hubConnection.On<string, object>("ReceiveMessage", async (conversationId, message) =>
        {
            if (OnMessageReceived != null)
            {
                await OnMessageReceived.Invoke(conversationId, message);
            }
        });
        
        _hubConnection.On<string, string, bool>("UserTyping", async (conversationId, userId, isTyping) =>
        {
            if (OnUserTyping != null)
            {
                await OnUserTyping.Invoke(conversationId, userId, isTyping);
            }
        });

        _hubConnection.On<string, string, string>("MessageRead", async (conversationId, messageId, userId) =>
        {
            if (OnMessageRead != null)
            {
                await OnMessageRead.Invoke(conversationId, messageId, userId);
            }
        });

        _hubConnection.Closed += async (error) =>
        {
            logger.LogWarning(error, "Messaging hub connection closed");
            await Task.Delay(new Random().Next(0, 5) * 1000);
        };

        _hubConnection.Reconnecting += (error) =>
        {
            logger.LogWarning(error, "Messaging hub reconnecting");
            return Task.CompletedTask;
        };

        _hubConnection.Reconnected += (connectionId) =>
        {
            logger.LogInformation("Messaging hub reconnected with ID: {ConnectionId}", connectionId);
            return Task.CompletedTask;
        };

        try
        {
            await _hubConnection.StartAsync();
            logger.LogInformation("Messaging hub connection started successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error starting messaging hub connection");
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task StopAsync()
    {
        if (_hubConnection is not null)
        {
            await _hubConnection.StopAsync();
            logger.LogInformation("SignalR connection stopped");
        }
    }

    /// <inheritdoc/>
    public async Task SendTypingIndicatorAsync(string conversationId, bool isTyping, List<string> participantIds)
    {
        if (_hubConnection is not null && IsConnected)
        {
            try
            {
                await _hubConnection.SendAsync("SendTypingIndicator", conversationId, isTyping, participantIds);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error sending typing indicator");
            }
        }
    }

    /// <inheritdoc/>
    public async Task SendMessageReadNotificationAsync(string conversationId, string messageId, List<string> participantIds)
    {
        if (_hubConnection is not null && IsConnected)
        {
            try
            {
                await _hubConnection.SendAsync("SendMessageReadNotification", conversationId, messageId, participantIds);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error sending message read notification");
            }
        }
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
            await _hubConnection.DisposeAsync();
        }

        GC.SuppressFinalize(this);
    }
}

