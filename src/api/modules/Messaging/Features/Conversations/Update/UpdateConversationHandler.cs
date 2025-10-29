namespace FSH.Starter.WebApi.Messaging.Features.Conversations.Update;

/// <summary>
/// Handler for updating a conversation.
/// </summary>
public sealed class UpdateConversationHandler(
    ILogger<UpdateConversationHandler> logger,
    [FromKeyedServices("messaging")] IRepository<Conversation> repository,
    ICurrentUser currentUser)
    : IRequestHandler<UpdateConversationCommand, UpdateConversationResponse>
{
    public async Task<UpdateConversationResponse> Handle(UpdateConversationCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var currentUserId = currentUser.GetUserId();

        var conversation = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        if (conversation == null)
        {
            throw new ConversationNotFoundException(request.Id);
        }

        // Verify user is an admin or creator
        var member = conversation.Members.FirstOrDefault(m => m.UserId == currentUserId && m.IsActive);
        if (member == null)
        {
            throw new UnauthorizedConversationAccessException(request.Id, currentUserId);
        }

        if (member.Role != MemberRoles.Admin && conversation.CreatedByUserId != currentUserId)
        {
            throw new UnauthorizedConversationAccessException(request.Id, currentUserId);
        }

        conversation.Update(request.Title, request.Description, request.ImageUrl);

        await repository.UpdateAsync(conversation, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("conversation {ConversationId} updated by user {UserId}", conversation.Id, currentUserId);

        return new UpdateConversationResponse(conversation.Id);
    }
}

