namespace FSH.Starter.WebApi.Messaging.Features.Messages.Delete;

public record DeleteMessageCommand(DefaultIdType Id) : IRequest;

