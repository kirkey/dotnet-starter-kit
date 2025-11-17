using Asp.Versioning;
using Shared.Authorization;

namespace FSH.Starter.WebApi.Messaging.Features.Messages.Create;

/// <summary>
/// Endpoint for creating a new message.
/// </summary>
public static class CreateMessageEndpoint
{
    internal static RouteHandlerBuilder MapCreateMessageEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (CreateMessageCommand request, ISender mediator) =>
                {
                    var response = await mediator.Send(request).ConfigureAwait(false);
                    return Results.CreatedAtRoute(nameof(CreateMessageEndpoint), new { id = response.Id }, response);
                })
                .WithName(nameof(CreateMessageEndpoint))
                .WithSummary("creates a new message")
                .WithDescription("creates a new message in a conversation with optional file attachments")
                .Produces<CreateMessageResponse>(StatusCodes.Status201Created)
                .RequirePermission(FshPermission.NameFor(FshActions.Create, FshResources.Messaging))
                .MapToApiVersion(new ApiVersion(1, 0));
    }
}
