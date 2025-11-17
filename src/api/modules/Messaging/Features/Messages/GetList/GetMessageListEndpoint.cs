using Microsoft.AspNetCore.Mvc;
using Shared.Authorization;

namespace FSH.Starter.WebApi.Messaging.Features.Messages.GetList;

public static class GetMessageListEndpoint
{
    internal static RouteHandlerBuilder MapGetMessageListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/conversations/{conversationId:guid}/messages/search", 
            async (DefaultIdType conversationId, ISender mediator, [FromBody] PaginationFilter filter) =>
            {
                var response = await mediator.Send(new GetMessageListRequest(conversationId, filter)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetMessageListEndpoint))
            .WithSummary("gets messages for a conversation with paging support")
            .WithDescription("gets a paginated list of messages for a conversation")
            .Produces<PagedList<MessageDto>>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Messaging))
            .MapToApiVersion(1);
    }
}

