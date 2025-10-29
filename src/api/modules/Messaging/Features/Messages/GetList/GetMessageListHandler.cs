namespace FSH.Starter.WebApi.Messaging.Features.Messages.GetList;

public sealed class GetMessageListHandler(
    ILogger<GetMessageListHandler> logger,
    [FromKeyedServices("messaging")] IReadRepository<Conversation> conversationRepository,
    MessagingDbContext context,
    ICurrentUser currentUser)
    : IRequestHandler<GetMessageListRequest, PagedList<MessageDto>>
{
    public async Task<PagedList<MessageDto>> Handle(GetMessageListRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var currentUserId = currentUser.GetUserId();

        // Verify conversation exists and user is a member
        var conversation = await conversationRepository.GetByIdAsync(request.ConversationId, cancellationToken).ConfigureAwait(false);
        if (conversation == null)
        {
            throw new ConversationNotFoundException(request.ConversationId);
        }

        var isMember = conversation.Members.Any(m => m.UserId == currentUserId && m.IsActive);
        if (!isMember)
        {
            throw new UnauthorizedConversationAccessException(request.ConversationId, currentUserId);
        }

        // Get messages for the conversation
        var query = context.Messages
            .Include(m => m.Attachments)
            .Where(m => m.ConversationId == request.ConversationId && !m.IsDeleted)
            .OrderByDescending(m => m.SentAt)
            .AsNoTracking();

        var totalCount = await query.CountAsync(cancellationToken).ConfigureAwait(false);

        var messages = await query
            .Skip((request.Filter.PageNumber - 1) * request.Filter.PageSize)
            .Take(request.Filter.PageSize)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        var messageDtos = messages.Select(m => new MessageDto(
            m.Id,
            m.SenderId,
            m.Content,
            m.MessageType,
            m.ReplyToMessageId,
            m.SentAt,
            m.IsEdited,
            m.IsDeleted,
            m.Attachments.Count)).ToList();

        logger.LogInformation("retrieved {Count} messages from conversation {ConversationId} for user {UserId}", 
            messageDtos.Count, request.ConversationId, currentUserId);

        return new PagedList<MessageDto>(messageDtos, totalCount, request.Filter.PageNumber, request.Filter.PageSize);
    }
}

