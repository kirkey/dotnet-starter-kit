namespace FSH.Starter.WebApi.Messaging.Features.Messages.Delete;

public sealed class DeleteMessageHandler(
    ILogger<DeleteMessageHandler> logger,
    [FromKeyedServices("messaging")] IRepository<Message> repository,
    ICurrentUser currentUser)
    : IRequestHandler<DeleteMessageCommand>
{
    public async Task Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var currentUserId = currentUser.GetUserId();

        var message = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        if (message == null)
        {
            throw new MessageNotFoundException(request.Id);
        }

        // Only sender can delete their own message
        if (message.SenderId != currentUserId)
        {
            throw new UnauthorizedMessageAccessException(request.Id, currentUserId);
        }

        message.Delete();

        await repository.UpdateAsync(message, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("message {MessageId} deleted by user {UserId}", message.Id, currentUserId);
    }
}

