namespace FSH.Starter.WebApi.Messaging.Features.Messages.Create;

/// <summary>
/// Response after creating a message.
/// </summary>
public record CreateMessageResponse(DefaultIdType Id, DateTime SentAt);

