using Shared.Authorization;

namespace FSH.Starter.WebApi.Messaging.Features.Conversations.Get;

/// <summary>
/// Endpoint for getting a conversation by ID.
/// </summary>
public static class GetConversationEndpoint
{
    internal static RouteHandlerBuilder MapGetConversationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
                {
                    var response = await mediator.Send(new GetConversationRequest(id)).ConfigureAwait(false);
                    return Results.Ok(response);
                })
                .WithName(nameof(GetConversationEndpoint))
                .WithSummary("gets conversation by id")
                .WithDescription("gets conversation details by id")
                .Produces<GetConversationResponse>()
                .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Messaging))
                .MapToApiVersion(1);
    }
}


