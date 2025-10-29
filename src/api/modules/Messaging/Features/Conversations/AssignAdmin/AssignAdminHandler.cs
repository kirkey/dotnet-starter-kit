namespace FSH.Starter.WebApi.Messaging.Features.Conversations.AssignAdmin;

/// <summary>
/// Handler for assigning or revoking admin role for a member.
/// </summary>
public sealed class AssignAdminHandler(
    ILogger<AssignAdminHandler> logger,
    [FromKeyedServices("messaging")] IRepository<Conversation> repository,
    ICurrentUser currentUser)
    : IRequestHandler<AssignAdminCommand>
{
    public async Task Handle(AssignAdminCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var currentUserId = currentUser.GetUserId();

        var conversation = await repository.GetByIdAsync(request.ConversationId, cancellationToken).ConfigureAwait(false);
        if (conversation == null)
        {
            throw new ConversationNotFoundException(request.ConversationId);
        }

        // Only existing admins can assign/revoke admin role
        var currentMember = conversation.Members.FirstOrDefault(m => m.UserId == currentUserId && m.IsActive);
        if (currentMember == null || currentMember.Role != MemberRoles.Admin)
        {
            throw new UnauthorizedConversationAccessException(request.ConversationId, currentUserId);
        }

        conversation.UpdateMemberRole(request.UserId, request.Role);

        await repository.UpdateAsync(conversation, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("user {UserId} role updated to {Role} in conversation {ConversationId} by {CurrentUserId}", 
            request.UserId, request.Role, conversation.Id, currentUserId);
    }
}

