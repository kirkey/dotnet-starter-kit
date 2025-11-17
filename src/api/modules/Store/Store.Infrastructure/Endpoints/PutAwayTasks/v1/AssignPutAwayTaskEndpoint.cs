using FSH.Starter.WebApi.Store.Application.PutAwayTasks.Assign.v1;
using Shared.Authorization;

namespace Store.Infrastructure.Endpoints.PutAwayTasks.v1;

public static class AssignPutAwayTaskEndpoint
{
    internal static RouteHandlerBuilder MapAssignPutAwayTaskEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/assign", async (DefaultIdType id, AssignPutAwayTaskCommand request, ISender sender) =>
            {
                if (id != request.PutAwayTaskId)
                {
                    return Results.BadRequest("Put-away task ID mismatch");
                }
                
                var response = await sender.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(AssignPutAwayTaskEndpoint))
            .WithSummary("Assign a put-away task to a worker")
            .WithDescription("Assigns a put-away task to a warehouse worker for execution.")
            .Produces<AssignPutAwayTaskResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.Update, FshResources.Warehouse))
            .MapToApiVersion(1);
    }
}
