namespace FSH.Starter.WebApi.Messaging.Exceptions;

/// <summary>
/// Exception thrown when a member is not found in a conversation.
/// </summary>
internal sealed class MemberNotFoundException(DefaultIdType conversationId, DefaultIdType userId)
    : NotFoundException($"member {userId} not found in conversation {conversationId}");

