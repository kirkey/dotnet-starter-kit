using Accounting.Application.Projects.Dashboard;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Projects.v1;

public static class ProjectDashboardEndpoint
{
    internal static RouteHandlerBuilder MapProjectDashboardEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/by-id/{id}/dashboard", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetProjectDashboardQuery(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(ProjectDashboardEndpoint))
            .WithSummary("Get project dashboard analytics")
            .WithDescription("Retrieves comprehensive dashboard data including budget vs actual, costs, revenue, timeline, and trends for a specific project")
            .Produces<ProjectDashboardResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
