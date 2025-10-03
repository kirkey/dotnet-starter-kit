using FSH.Starter.WebApi.Store.Application.PutAwayTasks.Search.v1;

namespace Store.Infrastructure.Endpoints.PutAwayTasks.v1;

public static class SearchPutAwayTasksEndpoint
{
    internal static RouteHandlerBuilder MapSearchPutAwayTasksEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchPutAwayTasksCommand request, IMediator mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchPutAwayTasksEndpoint))
            .WithSummary("Search put-away tasks")
            .WithDescription("Search put-away tasks with filters")
            .Produces<PagedList<PutAwayTaskResponse>>(200)
            .RequirePermission("store:putawaytasks:view")
            .MapToApiVersion(1);
    }
}
