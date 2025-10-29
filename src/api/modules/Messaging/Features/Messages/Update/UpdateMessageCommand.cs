namespace FSH.Starter.WebApi.Messaging.Features.Messages.Update;

public record UpdateMessageCommand(
    DefaultIdType Id,
    [property: DefaultValue("Updated message content")] string Content) : IRequest<UpdateMessageResponse>;

public record UpdateMessageResponse(DefaultIdType Id, DateTime EditedAt);

