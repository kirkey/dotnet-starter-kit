namespace FSH.Starter.WebApi.Messaging.Domain.Events;

/// <summary>
/// Event raised when a member is added to a conversation.
/// </summary>
public record MemberAdded(DefaultIdType ConversationId, DefaultIdType UserId, string Role) : DomainEvent;

