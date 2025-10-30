using FSH.Framework.Infrastructure.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace FSH.Starter.WebApi.Messaging.Hubs;

/// <summary>
/// SignalR hub for real-time messaging functionality.
/// Handles message sending, typing indicators, and read receipts for conversations.
/// Note: Connection tracking is handled by the separate ConnectionHub.
/// </summary>
[Authorize]
public class MessagingHub : Hub
{
    private readonly IConnectionTracker _connectionTracker;
    private readonly ILogger<MessagingHub> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="MessagingHub"/> class.
    /// </summary>
    /// <param name="connectionTracker">The connection tracker service for looking up user connections.</param>
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
    /// </summary>
    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        if (!string.IsNullOrEmpty(userId))
        {
            _logger.LogInformation("User {UserId} connected to messaging hub with connection {ConnectionId}", userId, Context.ConnectionId);
        }

        await base.OnConnectedAsync();
    }

    /// <summary>
    /// Called when a client disconnects from the hub.
    /// </summary>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.UserIdentifier;
        if (!string.IsNullOrEmpty(userId))
        {
            _logger.LogInformation("User {UserId} disconnected from messaging hub connection {ConnectionId}", userId, Context.ConnectionId);
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
}

