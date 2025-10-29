namespace FSH.Starter.WebApi.Messaging.Exceptions;

/// <summary>
/// Exception thrown when a conversation is not found.
/// </summary>
internal sealed class ConversationNotFoundException(DefaultIdType id)
    : NotFoundException($"conversation with id {id} not found");
