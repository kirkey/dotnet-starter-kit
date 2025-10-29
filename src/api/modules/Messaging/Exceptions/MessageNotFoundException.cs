namespace FSH.Starter.WebApi.Messaging.Exceptions;

/// <summary>
/// Exception thrown when a message is not found.
/// </summary>
internal sealed class MessageNotFoundException(DefaultIdType id)
    : NotFoundException($"message with id {id} not found");

