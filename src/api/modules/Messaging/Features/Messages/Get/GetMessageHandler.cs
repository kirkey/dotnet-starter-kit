namespace FSH.Starter.WebApi.Messaging.Features.Messages.Get;

public sealed class GetMessageHandler(
    ILogger<GetMessageHandler> logger,
    [FromKeyedServices("messaging")] IReadRepository<Message> repository,
    ICurrentUser currentUser)
    : IRequestHandler<GetMessageRequest, GetMessageResponse>
{
    public async Task<GetMessageResponse> Handle(GetMessageRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var message = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        if (message == null)
        {
            throw new MessageNotFoundException(request.Id);
        }

        // Verify user is a member of the conversation
        var currentUserId = currentUser.GetUserId();
        var isMember = message.Conversation.Members.Any(m => m.UserId == currentUserId && m.IsActive);
        if (!isMember)
        {
            throw new UnauthorizedConversationAccessException(message.ConversationId, currentUserId);
        }

        logger.LogInformation("retrieved message {MessageId} for user {UserId}", message.Id, currentUserId);

        var attachments = message.Attachments.Select(a => new AttachmentDto(
            a.Id, a.FileUrl, a.FileName, a.FileType, a.FileSize, a.ThumbnailUrl)).ToList();

        return new GetMessageResponse(
            message.Id,
            message.ConversationId,
            message.SenderId,
            message.Content,
            message.MessageType,
            message.ReplyToMessageId,
            message.SentAt,
            message.IsEdited,
            message.EditedAt,
            message.IsDeleted,
            attachments);
    }
}

