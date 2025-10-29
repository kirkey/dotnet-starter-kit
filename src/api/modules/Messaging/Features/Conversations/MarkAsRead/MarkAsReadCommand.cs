namespace FSH.Starter.WebApi.Messaging.Features.Conversations.MarkAsRead;

/// <summary>
/// Command for marking messages as read in a conversation.
/// Updates the LastReadAt timestamp for the current user.
/// </summary>
public record MarkAsReadCommand(DefaultIdType ConversationId) : IRequest;

