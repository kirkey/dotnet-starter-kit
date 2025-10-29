namespace FSH.Starter.WebApi.Messaging.Features.Conversations.GetList;
/// <summary>
/// Request for getting a paginated list of conversations for the current user.
/// </summary>
public record GetConversationListRequest(PaginationFilter Filter) : IRequest<PagedList<ConversationDto>>;
