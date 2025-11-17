using Asp.Versioning;
using Shared.Authorization;

namespace FSH.Starter.WebApi.Messaging.Features.Conversations.Create;

/// <summary>
/// Endpoint for creating a new conversation.
/// </summary>
public static class CreateConversationEndpoint
{
    internal static RouteHandlerBuilder MapCreateConversationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (CreateConversationCommand request, ISender mediator) =>
                {
                    var response = await mediator.Send(request).ConfigureAwait(false);
                    return Results.CreatedAtRoute(nameof(CreateConversationEndpoint), new { id = response.Id }, response);
                })
                .WithName(nameof(CreateConversationEndpoint))
                .WithSummary("creates a new conversation")
                .WithDescription("creates a new conversation (direct or group)")
                .Produces<CreateConversationResponse>(StatusCodes.Status201Created)
                .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Messaging))
                .MapToApiVersion(new ApiVersion(1, 0));
    }
}
