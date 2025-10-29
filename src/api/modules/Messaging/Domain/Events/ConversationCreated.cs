namespace FSH.Starter.WebApi.Messaging.Domain.Events;

/// <summary>
/// Event raised when a new conversation is created.
/// </summary>
public record ConversationCreated(DefaultIdType Id, string? Title, string ConversationType) : DomainEvent;
