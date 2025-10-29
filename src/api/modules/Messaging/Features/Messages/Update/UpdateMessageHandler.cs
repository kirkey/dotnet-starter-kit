namespace FSH.Starter.WebApi.Messaging.Features.Messages.Update;

public sealed class UpdateMessageHandler(
    ILogger<UpdateMessageHandler> logger,
    [FromKeyedServices("messaging")] IRepository<Message> repository,
    ICurrentUser currentUser)
    : IRequestHandler<UpdateMessageCommand, UpdateMessageResponse>
{
    public async Task<UpdateMessageResponse> Handle(UpdateMessageCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var currentUserId = currentUser.GetUserId();

        var message = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        if (message == null)
        {
            throw new MessageNotFoundException(request.Id);
        }

        // Only sender can update their own message
        if (message.SenderId != currentUserId)
        {
            throw new UnauthorizedMessageAccessException(request.Id, currentUserId);
        }

        message.Update(request.Content);

        await repository.UpdateAsync(message, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("message {MessageId} updated by user {UserId}", message.Id, currentUserId);

        return new UpdateMessageResponse(message.Id, message.EditedAt!.Value);
    }
}

