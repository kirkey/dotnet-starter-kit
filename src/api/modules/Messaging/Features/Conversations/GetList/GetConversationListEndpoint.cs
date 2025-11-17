using Microsoft.AspNetCore.Mvc;
using Shared.Authorization;

namespace FSH.Starter.WebApi.Messaging.Features.Conversations.GetList;

/// <summary>
/// Endpoint for getting a paginated list of conversations.
/// </summary>
public static class GetConversationListEndpoint
{
    internal static RouteHandlerBuilder MapGetConversationListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/search", async (ISender mediator, [FromBody] PaginationFilter filter) =>
        {
            var response = await mediator.Send(new GetConversationListRequest(filter)).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(GetConversationListEndpoint))
        .WithSummary("gets a list of conversations with paging support")
        .WithDescription("gets a list of conversations for the current user with paging support")
        .Produces<PagedList<ConversationDto>>()
        .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Messaging))
        .MapToApiVersion(1);
    }
}
