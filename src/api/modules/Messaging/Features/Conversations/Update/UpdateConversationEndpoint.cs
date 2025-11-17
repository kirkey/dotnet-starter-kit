using Shared.Authorization;

namespace FSH.Starter.WebApi.Messaging.Features.Conversations.Update;

/// <summary>
/// Endpoint for updating a conversation.
/// </summary>
public static class UpdateConversationEndpoint
{
    internal static RouteHandlerBuilder MapUpdateConversationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (DefaultIdType id, UpdateConversationCommand request, ISender mediator) =>
        {
            if (id != request.Id)
            {
                return Results.BadRequest("id in route does not match id in request body");
            }

            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(UpdateConversationEndpoint))
        .WithSummary("updates a conversation")
        .WithDescription("updates conversation details")
        .Produces<UpdateConversationResponse>()
        .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Messaging))
        .MapToApiVersion(1);
    }
}
