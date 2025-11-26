using FSH.Starter.WebApi.Store.Application.PutAwayTasks.Delete.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.PutAwayTasks.v1;

public static class DeletePutAwayTaskEndpoint
{
    internal static RouteHandlerBuilder MapDeletePutAwayTaskEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (DefaultIdType id, ISender sender) =>
            {
                await sender.Send(new DeletePutAwayTaskCommand { PutAwayTaskId = id }).ConfigureAwait(false);
                return Results.NoContent();
            })
            .WithName(nameof(DeletePutAwayTaskEndpoint))
            .WithSummary("Delete a put-away task")
            .WithDescription("Deletes a put-away task and all associated items.")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission(FshPermission.NameFor(FshActions.Delete, FshResources.Warehouse))
            .MapToApiVersion(1);
    }
}
