using FSH.Starter.WebApi.Store.Application.PutAwayTasks.Assign.v1;

namespace Store.Infrastructure.Endpoints.PutAwayTasks.v1;

public static class AssignPutAwayTaskEndpoint
{
    internal static RouteHandlerBuilder MapAssignPutAwayTaskEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/assign", async (DefaultIdType id, AssignPutAwayTaskCommand request, IMediator mediator) =>
            {
                if (id != request.PutAwayTaskId)
                {
                    request = request with { PutAwayTaskId = id };
                }
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(AssignPutAwayTaskEndpoint))
            .WithSummary("Assign a put-away task to a worker")
            .WithDescription("Assign a put-away task to a worker")
            .Produces<AssignPutAwayTaskResponse>(200)
            .RequirePermission("store:putawaytasks:update")
            .MapToApiVersion(1);
    }
}
