namespace FSH.Starter.WebApi.Messaging.Exceptions;

/// <summary>
/// Exception thrown when a user is not authorized to perform an action on a conversation.
/// </summary>
internal sealed class UnauthorizedConversationAccessException(DefaultIdType conversationId, DefaultIdType userId)
    : ForbiddenException($"user {userId} is not authorized to access conversation {conversationId}");

