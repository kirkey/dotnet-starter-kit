using FSH.Framework.Infrastructure.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace FSH.Starter.WebApi.Messaging.Hubs;

/// <summary>
/// SignalR hub for real-time messaging functionality.
/// Handles message sending, online status tracking, and typing indicators.
/// </summary>
[Authorize]
public class MessagingHub : Hub
{
    private readonly IConnectionTracker _connectionTracker;
    private readonly ILogger<MessagingHub> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="MessagingHub"/> class.
    /// </summary>
    /// <param name="connectionTracker">The connection tracker service.</param>
    /// <param name="logger">The logger instance.</param>
    public MessagingHub(
        IConnectionTracker connectionTracker,
        ILogger<MessagingHub> logger)
    {
        _connectionTracker = connectionTracker;
        _logger = logger;
    }

    /// <summary>
    /// Called when a client connects to the hub.
    /// Tracks the connection and notifies other users of online status.
    /// </summary>
    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        if (!string.IsNullOrEmpty(userId))
        {
            await _connectionTracker.AddConnectionAsync(userId, Context.ConnectionId);
            
            // Notify all clients about user online status
            await Clients.Others.SendAsync("UserOnline", userId);
            
            _logger.LogInformation("User {UserId} connected with connection {ConnectionId}", userId, Context.ConnectionId);
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
            await _connectionTracker.RemoveConnectionAsync(userId, Context.ConnectionId);
            
            // Check if user still has other active connections
            var isStillOnline = await _connectionTracker.IsUserOnlineAsync(userId);
            if (!isStillOnline)
            {
                // Notify all clients that user is offline
                await Clients.Others.SendAsync("UserOffline", userId);
            }
            
            _logger.LogInformation("User {UserId} disconnected from connection {ConnectionId}", userId, Context.ConnectionId);
        }

        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// Sends a message notification to all participants in a conversation.
    /// </summary>
    /// <param name="conversationId">The conversation identifier.</param>
    /// <param name="message">The message object to send.</param>
    /// <param name="participantIds">The list of participant user IDs.</param>
    public async Task SendMessageToConversation(string conversationId, object message, IEnumerable<string> participantIds)
    {
        ArgumentNullException.ThrowIfNull(conversationId);
        ArgumentNullException.ThrowIfNull(message);
        ArgumentNullException.ThrowIfNull(participantIds);

        var userId = Context.UserIdentifier;
        
        // Send to all participants except the sender
        var otherParticipants = participantIds.Where(p => p != userId).ToList();
        
        foreach (var participantId in otherParticipants)
        {
            var connections = (await _connectionTracker.GetConnectionsAsync(participantId)).ToList();
            if (connections.Count > 0)
            {
                await Clients.Clients(connections).SendAsync("ReceiveMessage", conversationId, message);
            }
        }

        _logger.LogInformation("Message sent to conversation {ConversationId} by user {UserId}", conversationId, userId);
    }

    /// <summary>
    /// Sends a typing indicator to participants in a conversation.
    /// </summary>
    /// <param name="conversationId">The conversation identifier.</param>
    /// <param name="isTyping">Whether the user is typing.</param>
    /// <param name="participantIds">The list of participant user IDs.</param>
    public async Task SendTypingIndicator(string conversationId, bool isTyping, IEnumerable<string> participantIds)
    {
        ArgumentNullException.ThrowIfNull(conversationId);
        ArgumentNullException.ThrowIfNull(participantIds);

        var userId = Context.UserIdentifier;
        
        // Send to all participants except the sender
        var otherParticipants = participantIds.Where(p => p != userId).ToList();
        
        foreach (var participantId in otherParticipants)
        {
            var connections = (await _connectionTracker.GetConnectionsAsync(participantId)).ToList();
            if (connections.Count > 0)
            {
                await Clients.Clients(connections).SendAsync("UserTyping", conversationId, userId, isTyping);
            }
        }
    }

    /// <summary>
    /// Notifies participants that a message has been read.
    /// </summary>
    /// <param name="conversationId">The conversation identifier.</param>
    /// <param name="messageId">The message identifier.</param>
    /// <param name="participantIds">The list of participant user IDs.</param>
    public async Task SendMessageReadNotification(string conversationId, string messageId, IEnumerable<string> participantIds)
    {
        ArgumentNullException.ThrowIfNull(conversationId);
        ArgumentNullException.ThrowIfNull(messageId);
        ArgumentNullException.ThrowIfNull(participantIds);

        var userId = Context.UserIdentifier;
        
        // Send to all participants except the sender
        var otherParticipants = participantIds.Where(p => p != userId).ToList();
        
        foreach (var participantId in otherParticipants)
        {
            var connections = (await _connectionTracker.GetConnectionsAsync(participantId)).ToList();
            if (connections.Count > 0)
            {
                await Clients.Clients(connections).SendAsync("MessageRead", conversationId, messageId, userId);
            }
        }
    }

    /// <summary>
    /// Gets the list of currently online users.
    /// </summary>
    /// <returns>A list of online user IDs.</returns>
    public async Task<IEnumerable<string>> GetOnlineUsers()
    {
        return await _connectionTracker.GetOnlineUsersAsync();
    }
}

