namespace FSH.Starter.WebApi.Messaging.Features.Conversations.GetList;

/// <summary>
/// Handler for getting a paginated list of conversations for the current user.
/// </summary>
public sealed class GetConversationListHandler(
    ILogger<GetConversationListHandler> logger,
    MessagingDbContext context,
    ICurrentUser currentUser)
    : IRequestHandler<GetConversationListRequest, PagedList<ConversationDto>>
{
    public async Task<PagedList<ConversationDto>> Handle(GetConversationListRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var currentUserId = currentUser.GetUserId();

        // Get conversations where user is an active member
        var query = context.Conversations
            .Include(c => c.Members)
            .Include(c => c.Messages.OrderByDescending(m => m.SentAt).Take(1))
            .Where(c => c.Members.Any(m => m.UserId == currentUserId && m.IsActive) && c.IsActive)
            .OrderByDescending(c => c.LastMessageAt ?? c.CreatedOn)
            .AsNoTracking();

        var totalCount = await query.CountAsync(cancellationToken).ConfigureAwait(false);

        var conversations = await query
            .Skip((request.Filter.PageNumber - 1) * request.Filter.PageSize)
            .Take(request.Filter.PageSize)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        var conversationDtos = conversations.Select(c =>
        {
            var member = c.Members.First(m => m.UserId == currentUserId);
            var lastMessage = c.Messages.FirstOrDefault();
            var unreadCount = c.Messages.Count(m => !m.IsDeleted && m.SentAt > (member.LastReadAt ?? DateTime.MinValue));

            return new ConversationDto(
                c.Id,
                c.Title,
                c.ConversationType,
                c.ImageUrl,
                c.LastMessageAt,
                unreadCount,
                lastMessage?.Content,
                c.CreatedOn);
        }).ToList();

        logger.LogInformation("retrieved {Count} conversations for user {UserId}", conversationDtos.Count, currentUserId);

        return new PagedList<ConversationDto>(conversationDtos, totalCount, request.Filter.PageNumber, request.Filter.PageSize);
    }
}

