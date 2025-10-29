namespace FSH.Starter.WebApi.Messaging.Domain.Events;

/// <summary>
/// Event raised when a message is sent.
/// </summary>
public record MessageSent(DefaultIdType MessageId, DefaultIdType ConversationId, DefaultIdType SenderId, string Content) : DomainEvent;

