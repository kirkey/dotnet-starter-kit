namespace FSH.Starter.WebApi.Messaging.Features.Conversations.Get;

/// <summary>
/// Handler for getting a conversation by ID.
/// </summary>
public sealed class GetConversationHandler(
    ILogger<GetConversationHandler> logger,
    [FromKeyedServices("messaging")] IReadRepository<Conversation> repository,
    ICurrentUser currentUser)
    : IRequestHandler<GetConversationRequest, GetConversationResponse>
{
    public async Task<GetConversationResponse> Handle(GetConversationRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var currentUserId = currentUser.GetUserId();

        var conversation = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        if (conversation == null)
        {
            throw new ConversationNotFoundException(request.Id);
        }

        // Verify user is a member of the conversation
        var isMember = conversation.Members.Any(m => m.UserId == currentUserId && m.IsActive);
        if (!isMember)
        {
            throw new UnauthorizedConversationAccessException(request.Id, currentUserId);
        }

        logger.LogInformation("retrieved conversation {ConversationId} for user {UserId}", conversation.Id, currentUserId);

        var members = conversation.Members
            .Where(m => m.IsActive)
            .Select(m => new ConversationMemberDto(
                m.UserId,
                m.Role,
                m.IsActive,
                m.JoinedAt,
                m.LastReadAt))
            .ToList();

        return new GetConversationResponse(
            conversation.Id,
            conversation.Title,
            conversation.ConversationType,
            conversation.Description,
            conversation.ImageUrl,
            conversation.IsActive,
            conversation.LastMessageAt,
            members,
            conversation.CreatedOn);
    }
}

