namespace FSH.Starter.WebApi.Messaging.Features.Conversations.RemoveMember;

/// <summary>
/// Handler for removing a member from a conversation.
/// </summary>
public sealed class RemoveMemberHandler(
    ILogger<RemoveMemberHandler> logger,
    [FromKeyedServices("messaging")] IRepository<Conversation> repository,
    ICurrentUser currentUser)
    : IRequestHandler<RemoveMemberCommand>
{
    public async Task Handle(RemoveMemberCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var currentUserId = currentUser.GetUserId();

        var conversation = await repository.GetByIdAsync(request.ConversationId, cancellationToken).ConfigureAwait(false);
        if (conversation == null)
        {
            throw new ConversationNotFoundException(request.ConversationId);
        }

        // Only admins can remove members, or users can remove themselves
        if (request.UserId != currentUserId)
        {
            var member = conversation.Members.FirstOrDefault(m => m.UserId == currentUserId && m.IsActive);
            if (member == null || member.Role != MemberRoles.Admin)
            {
                throw new UnauthorizedConversationAccessException(request.ConversationId, currentUserId);
            }
        }

        conversation.RemoveMember(request.UserId);

        await repository.UpdateAsync(conversation, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("user {UserId} removed from conversation {ConversationId} by {CurrentUserId}", 
            request.UserId, conversation.Id, currentUserId);
    }
}

