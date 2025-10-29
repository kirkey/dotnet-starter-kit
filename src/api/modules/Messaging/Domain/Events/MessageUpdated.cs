namespace FSH.Starter.WebApi.Messaging.Domain.Events;

/// <summary>
/// Event raised when a message is updated.
/// </summary>
public record MessageUpdated(DefaultIdType MessageId, DefaultIdType ConversationId, string Content) : DomainEvent;

