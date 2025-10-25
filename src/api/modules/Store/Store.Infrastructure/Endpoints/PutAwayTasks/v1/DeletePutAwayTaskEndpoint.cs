using FSH.Starter.WebApi.Store.Application.PutAwayTasks.Delete.v1;

namespace Store.Infrastructure.Endpoints.PutAwayTasks.v1;

public static class DeletePutAwayTaskEndpoint
{
    internal static RouteHandlerBuilder MapDeletePutAwayTaskEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id}", async (DefaultIdType id, ISender sender) =>
            {
                var request = new DeletePutAwayTaskCommand { PutAwayTaskId = id };
                var response = await sender.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DeletePutAwayTaskEndpoint))
            .WithSummary("Delete a put-away task")
            .WithDescription("Deletes a put-away task and all associated items.")
            .Produces<DeletePutAwayTaskResponse>(200)
            .RequirePermission("Permissions.Store.Delete")
            .MapToApiVersion(1);
    }
}
