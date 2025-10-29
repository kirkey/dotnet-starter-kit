namespace FSH.Starter.WebApi.Messaging.Features.Conversations.MarkAsRead;

/// <summary>
/// Handler for marking messages as read in a conversation.
/// Updates the LastReadAt timestamp for the current user's membership.
/// </summary>
public sealed class MarkAsReadHandler(
    ILogger<MarkAsReadHandler> logger,
    [FromKeyedServices("messaging")] IRepository<Conversation> repository,
    ICurrentUser currentUser)
    : IRequestHandler<MarkAsReadCommand>
{
    public async Task Handle(MarkAsReadCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var currentUserId = currentUser.GetUserId();

        var conversation = await repository.GetByIdAsync(request.ConversationId, cancellationToken).ConfigureAwait(false);
        if (conversation == null)
        {
            throw new ConversationNotFoundException(request.ConversationId);
        }

        // Verify user is a member
        var member = conversation.Members.FirstOrDefault(m => m.UserId == currentUserId && m.IsActive);
        if (member == null)
        {
            throw new UnauthorizedConversationAccessException(request.ConversationId, currentUserId);
        }

        // Update last read timestamp
        member.UpdateLastRead(DateTime.UtcNow);

        await repository.UpdateAsync(conversation, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("conversation {ConversationId} marked as read by user {UserId}", 
            conversation.Id, currentUserId);
    }
}

