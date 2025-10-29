namespace FSH.Starter.WebApi.Messaging.Domain.Events;

/// <summary>
/// Event raised when a message is deleted.
/// </summary>
public record MessageDeleted(DefaultIdType MessageId, DefaultIdType ConversationId) : DomainEvent;

