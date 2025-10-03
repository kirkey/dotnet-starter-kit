namespace Store.Infrastructure.Endpoints.v1;

public static class SearchPutAwayTasksEndpoint
{
    internal static RouteHandlerBuilder MapSearchPutAwayTasksEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchPutAwayTasksRequest request, IMediator mediator) =>
            {
                var response = await mediator.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchPutAwayTasksEndpoint))
            .WithSummary("Search put-away tasks")
            .WithDescription("Search put-away tasks with filters")
            .Produces<PagedList<PutAwayTaskDto>>(200)
            .RequirePermission("store:putawaytasks:view")
            .MapToApiVersion(1);
    }
}
