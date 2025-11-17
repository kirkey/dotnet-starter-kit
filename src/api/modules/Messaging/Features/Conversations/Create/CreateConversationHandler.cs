namespace FSH.Starter.WebApi.Messaging.Features.Conversations.Create;

/// <summary>
/// Handler for creating a new conversation.
/// Implements CQRS pattern by handling CreateConversationCommand.
/// </summary>
public sealed class CreateConversationHandler(
    ILogger<CreateConversationHandler> logger,
    [FromKeyedServices("messaging")] IRepository<Conversation> repository,
    ICurrentUser currentUser)
    : IRequestHandler<CreateConversationCommand, CreateConversationResponse>
{
    public async Task<CreateConversationResponse> Handle(CreateConversationCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var currentUserId = currentUser.GetUserId();

        Conversation conversation;

        if (request.ConversationType == ConversationTypes.Direct)
        {
            // Create direct conversation between two users
            if (request.MemberIds == null || request.MemberIds.Count != 2)
            {
                throw new InvalidOperationException("direct conversations require exactly 2 members");
            }

            conversation = Conversation.CreateDirect(
                request.MemberIds[0],
                request.MemberIds[1],
                currentUserId);
        }
        else
        {
            // Create group conversation
            conversation = Conversation.CreateGroup(
                request.Title!,
                currentUserId,
                request.Description,
                request.ImageUrl);

            // Add additional members if specified
            if (request.MemberIds != null)
            {
                foreach (var memberId in request.MemberIds)
                {
                    if (memberId != currentUserId)
                    {
                        conversation.AddMember(memberId);
                    }
                }
            }
        }

        await repository.AddAsync(conversation, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("conversation created {ConversationId} by user {UserId}", conversation.Id, currentUserId);

        return new CreateConversationResponse(conversation.Id);
    }
}

