using Accounting.Application.ProjectCosting.Get.v1;
using Accounting.Application.ProjectCosting.Search.v1;

namespace Accounting.Infrastructure.Endpoints.ProjectCosting.v1;

/// <summary>
/// Endpoint for searching project costing entries with filters.
/// </summary>
public static class SearchProjectCostingEndpoint
{
    /// <summary>
    /// Maps the search project costing endpoint to the route builder.
    /// </summary>
    internal static RouteHandlerBuilder MapSearchProjectCostingEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/search", async (SearchProjectCostingQuery request, ISender mediator) =>
        {
            var response = await mediator.Send(request).ConfigureAwait(false);
            return Results.Ok(response);
        })
        .WithName(nameof(SearchProjectCostingEndpoint))
        .WithSummary("Search project costing entries")
        .WithDescription("Searches project costing entries with filters and pagination")
        .Produces<PagedList<ProjectCostingResponse>>()
        .RequirePermission("Permissions.Accounting.View")
        .MapToApiVersion(1);
    }
}
