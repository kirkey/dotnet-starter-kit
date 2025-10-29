namespace FSH.Starter.WebApi.Messaging.Features.Conversations.Delete;

/// <summary>
/// Handler for deleting a conversation (soft delete).
/// </summary>
public sealed class DeleteConversationHandler(
    ILogger<DeleteConversationHandler> logger,
    [FromKeyedServices("messaging")] IRepository<Conversation> repository,
    ICurrentUser currentUser)
    : IRequestHandler<DeleteConversationCommand>
{
    public async Task Handle(DeleteConversationCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var currentUserId = currentUser.GetUserId();

        var conversation = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        if (conversation == null)
        {
            throw new ConversationNotFoundException(request.Id);
        }

        // Only admin or creator can delete
        var member = conversation.Members.FirstOrDefault(m => m.UserId == currentUserId && m.IsActive);
        if (member == null || (member.Role != MemberRoles.Admin && conversation.CreatedByUserId != currentUserId))
        {
            throw new UnauthorizedConversationAccessException(request.Id, currentUserId);
        }

        conversation.Deactivate();

        await repository.UpdateAsync(conversation, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("conversation {ConversationId} deleted by user {UserId}", conversation.Id, currentUserId);
    }
}

