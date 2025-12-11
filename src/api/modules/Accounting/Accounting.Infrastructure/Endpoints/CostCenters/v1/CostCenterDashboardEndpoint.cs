using Accounting.Application.CostCenters.Dashboard;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.CostCenters.v1;

public static class CostCenterDashboardEndpoint
{
    internal static RouteHandlerBuilder MapCostCenterDashboardEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}/dashboard", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetCostCenterDashboardQuery(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CostCenterDashboardEndpoint))
            .WithSummary("Get cost center dashboard analytics")
            .WithDescription("Retrieves comprehensive dashboard data including budget vs actual, transactions, child cost centers, and trends for a specific cost center")
            .Produces<CostCenterDashboardResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
