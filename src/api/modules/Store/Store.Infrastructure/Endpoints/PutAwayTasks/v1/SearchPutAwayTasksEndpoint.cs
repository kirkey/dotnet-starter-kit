using FSH.Starter.WebApi.Store.Application.PutAwayTasks.Search.v1;

namespace Store.Infrastructure.Endpoints.PutAwayTasks.v1;

public static class SearchPutAwayTasksEndpoint
{
    internal static RouteHandlerBuilder MapSearchPutAwayTasksEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (SearchPutAwayTasksCommand request, ISender sender) =>
            {
                var response = await sender.Send(request).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchPutAwayTasksEndpoint))
            .WithSummary("Search put-away tasks")
            .WithDescription("Searches put-away tasks with filtering, sorting, and pagination.")
            .Produces<PagedList<PutAwayTaskResponse>>(200)
            .RequirePermission("store:putawaytasks:view")
            .MapToApiVersion(1);
    }
}
