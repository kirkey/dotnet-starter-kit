namespace FSH.Starter.WebApi.Messaging.Domain.Events;

/// <summary>
/// Event raised when a conversation is deleted.
/// </summary>
public record ConversationDeleted(DefaultIdType Id) : DomainEvent;

