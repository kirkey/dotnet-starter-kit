namespace FSH.Starter.WebApi.Messaging.Features.Conversations.AddMember;

/// <summary>
/// Handler for adding a member to a conversation.
/// </summary>
public sealed class AddMemberHandler(
    ILogger<AddMemberHandler> logger,
    [FromKeyedServices("messaging")] IRepository<Conversation> repository,
    ICurrentUser currentUser)
    : IRequestHandler<AddMemberCommand>
{
    public async Task Handle(AddMemberCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var currentUserId = currentUser.GetUserId();

        var conversation = await repository.GetByIdAsync(request.ConversationId, cancellationToken).ConfigureAwait(false);
        if (conversation == null)
        {
            throw new ConversationNotFoundException(request.ConversationId);
        }

        // Only admins can add members to groups
        if (conversation.ConversationType == ConversationTypes.Group)
        {
            var member = conversation.Members.FirstOrDefault(m => m.UserId == currentUserId && m.IsActive);
            if (member == null || member.Role != MemberRoles.Admin)
            {
                throw new UnauthorizedConversationAccessException(request.ConversationId, currentUserId);
            }
        }

        conversation.AddMember(request.UserId, request.Role);

        await repository.UpdateAsync(conversation, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("user {UserId} added to conversation {ConversationId} by {CurrentUserId}", 
            request.UserId, conversation.Id, currentUserId);
    }
}

