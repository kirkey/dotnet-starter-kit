using FSH.Starter.WebApi.Store.Application.PutAwayTasks.Get.v1;

namespace Store.Infrastructure.Endpoints.v1;

public static class GetPutAwayTaskEndpoint
{
    internal static RouteHandlerBuilder MapGetPutAwayTaskEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id}", async (DefaultIdType id, IMediator mediator) =>
            {
                var request = new GetPutAwayTaskRequest { PutAwayTaskId = id };
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(GetPutAwayTaskEndpoint))
            .WithSummary("Get a put-away task by ID")
            .WithDescription("Get a put-away task by ID")
            .Produces<GetPutAwayTaskResponse>(200)
            .RequirePermission("store:putawaytasks:view")
            .MapToApiVersion(1);
    }
}
