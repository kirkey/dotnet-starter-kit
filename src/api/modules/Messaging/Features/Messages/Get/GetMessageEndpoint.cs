using Shared.Authorization;

namespace FSH.Starter.WebApi.Messaging.Features.Messages.Get;

public static class GetMessageEndpoint
{
    internal static RouteHandlerBuilder MapGetMessageEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
                {
                    var response = await mediator.Send(new GetMessageRequest(id)).ConfigureAwait(false);
                    return Results.Ok(response);
                })
                .WithName(nameof(GetMessageEndpoint))
                .WithSummary("gets message by id")
                .WithDescription("gets message details by id")
                .Produces<GetMessageResponse>()
                .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Messaging))
                .MapToApiVersion(1);
    }
}

