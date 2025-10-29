namespace FSH.Starter.WebApi.Messaging.Domain.Events;

/// <summary>
/// Event raised when a conversation is updated.
/// </summary>
public record ConversationUpdated(DefaultIdType Id, string? Title, string? Description) : DomainEvent;

