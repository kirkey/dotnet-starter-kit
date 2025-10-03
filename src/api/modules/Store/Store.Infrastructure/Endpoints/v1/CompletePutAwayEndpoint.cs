using FSH.Starter.WebApi.Store.Application.PutAwayTasks.Complete.v1;

namespace Store.Infrastructure.Endpoints.v1;

public static class CompletePutAwayEndpoint
{
    internal static RouteHandlerBuilder MapCompletePutAwayEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id}/complete", async (DefaultIdType id, IMediator mediator) =>
            {
                var request = new CompletePutAwayCommand { PutAwayTaskId = id };
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CompletePutAwayEndpoint))
            .WithSummary("Complete a put-away task")
            .WithDescription("Complete a put-away task")
            .Produces<CompletePutAwayResponse>(200)
            .RequirePermission("store:putawaytasks:update")
            .MapToApiVersion(1);
    }
}
