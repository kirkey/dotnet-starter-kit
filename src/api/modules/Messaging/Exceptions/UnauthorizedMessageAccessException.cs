namespace FSH.Starter.WebApi.Messaging.Exceptions;

/// <summary>
/// Exception thrown when a user is not authorized to perform an action on a message.
/// </summary>
internal sealed class UnauthorizedMessageAccessException(DefaultIdType messageId, DefaultIdType userId)
    : ForbiddenException($"user {userId} is not authorized to modify message {messageId}");

