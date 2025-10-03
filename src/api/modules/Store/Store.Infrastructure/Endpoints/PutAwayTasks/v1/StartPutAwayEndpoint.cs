using FSH.Starter.WebApi.Store.Application.PutAwayTasks.Start.v1;

namespace Store.Infrastructure.Endpoints.PutAwayTasks.v1;

public static class StartPutAwayEndpoint
{
    internal static RouteHandlerBuilder MapStartPutAwayEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/start", async (DefaultIdType id, IMediator mediator) =>
            {
                var request = new StartPutAwayCommand { PutAwayTaskId = id };
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(StartPutAwayEndpoint))
            .WithSummary("Start a put-away task")
            .WithDescription("Start a put-away task")
            .Produces<StartPutAwayResponse>(200)
            .RequirePermission("store:putawaytasks:update")
            .MapToApiVersion(1);
    }
}
