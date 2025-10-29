namespace FSH.Starter.WebApi.Messaging.Exceptions;

/// <summary>
/// Exception thrown when a file attachment is not found.
/// </summary>
internal sealed class AttachmentNotFoundException(DefaultIdType id)
    : NotFoundException($"attachment with id {id} not found");

