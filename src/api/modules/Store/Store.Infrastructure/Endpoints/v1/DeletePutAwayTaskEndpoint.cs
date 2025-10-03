using FSH.Starter.WebApi.Store.Application.PutAwayTasks.Delete.v1;

namespace Store.Infrastructure.Endpoints.v1;

public static class DeletePutAwayTaskEndpoint
{
    internal static RouteHandlerBuilder MapDeletePutAwayTaskEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id}", async (DefaultIdType id, IMediator mediator) =>
            {
                var request = new DeletePutAwayTaskCommand { PutAwayTaskId = id };
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(DeletePutAwayTaskEndpoint))
            .WithSummary("Delete a put-away task")
            .WithDescription("Delete a put-away task")
            .Produces<DeletePutAwayTaskResponse>(200)
            .RequirePermission("store:putawaytasks:delete")
            .MapToApiVersion(1);
    }
}
