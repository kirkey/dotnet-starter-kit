namespace FSH.Starter.WebApi.Messaging.Domain.Events;

/// <summary>
/// Event raised when a member's role is updated in a conversation.
/// </summary>
public record MemberRoleUpdated(DefaultIdType ConversationId, DefaultIdType UserId, string NewRole) : DomainEvent;
