using Shared.Authorization;

namespace FSH.Starter.WebApi.Messaging.Features.Messages.Delete;

public static class DeleteMessageEndpoint
{
    internal static RouteHandlerBuilder MapDeleteMessageEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (DefaultIdType id, ISender mediator) =>
        {
            await mediator.Send(new DeleteMessageCommand(id)).ConfigureAwait(false);
            return Results.NoContent();
        })
        .WithName(nameof(DeleteMessageEndpoint))
        .WithSummary("deletes a message")
        .WithDescription("soft deletes a message")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Messaging))
        .MapToApiVersion(1);
    }
}

