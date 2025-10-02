using Accounting.Application.Projects.Costing.Responses;
using Accounting.Application.Projects.Costing.Search;

namespace Accounting.Infrastructure.Endpoints.Projects.Costing.v1;

public static class ProjectCostingSearchEndpoint
{
    internal static RouteHandlerBuilder MapProjectCostingSearchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchProjectCostingsQuery command) =>
            {
                var response = await mediator.Send(command).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(ProjectCostingSearchEndpoint))
            .WithSummary("search project costing entries")
            .WithDescription("Gets a paginated list of project costing entries with filtering support")
            .Produces<PagedList<ProjectCostingResponse>>()
            .RequirePermission("Permissions.Accounting.View")
            .MapToApiVersion(1);
    }
}
