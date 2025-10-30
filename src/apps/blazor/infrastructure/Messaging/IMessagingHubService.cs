namespace FSH.Starter.Blazor.Infrastructure.Messaging;

/// <summary>
/// Interface for managing SignalR messaging hub connections.
/// Provides real-time messaging, online status, and typing indicators.
/// </summary>
public interface IMessagingHubService : IAsyncDisposable
{
    /// <summary>
    /// Event triggered when a new message is received.
    /// </summary>
    event Func<string, object, Task>? OnMessageReceived;

    /// <summary>
    /// Event triggered when a user comes online.
    /// </summary>
    event Func<string, Task>? OnUserOnline;

    /// <summary>
    /// Event triggered when a user goes offline.
    /// </summary>
    event Func<string, Task>? OnUserOffline;

    /// <summary>
    /// Event triggered when a user is typing.
    /// </summary>
    event Func<string, string, bool, Task>? OnUserTyping;

    /// <summary>
    /// Event triggered when a message is read.
    /// </summary>
    event Func<string, string, string, Task>? OnMessageRead;

    /// <summary>
    /// Gets whether the hub connection is currently active.
    /// </summary>
    bool IsConnected { get; }

    /// <summary>
    /// Starts the SignalR connection to the messaging hub.
    /// </summary>
    Task StartAsync();

    /// <summary>
    /// Stops the SignalR connection.
    /// </summary>
    Task StopAsync();

    /// <summary>
    /// Sends a typing indicator to conversation participants.
    /// </summary>
    /// <param name="conversationId">The conversation identifier.</param>
    /// <param name="isTyping">Whether the user is typing.</param>
    /// <param name="participantIds">List of participant user IDs.</param>
    Task SendTypingIndicatorAsync(string conversationId, bool isTyping, List<string> participantIds);

    /// <summary>
    /// Sends a message read notification.
    /// </summary>
    /// <param name="conversationId">The conversation identifier.</param>
    /// <param name="messageId">The message identifier.</param>
    /// <param name="participantIds">List of participant user IDs.</param>
    Task SendMessageReadNotificationAsync(string conversationId, string messageId, List<string> participantIds);

    /// <summary>
    /// Gets the list of currently online users.
    /// </summary>
    Task<IEnumerable<string>> GetOnlineUsersAsync();
}
