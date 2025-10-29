namespace FSH.Starter.WebApi.Messaging.Domain.Events;

/// <summary>
/// Event raised when a member is removed from a conversation.
/// </summary>
public record MemberRemoved(DefaultIdType ConversationId, DefaultIdType UserId) : DomainEvent;

