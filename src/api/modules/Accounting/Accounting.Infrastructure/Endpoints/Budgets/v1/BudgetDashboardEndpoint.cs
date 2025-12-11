using Accounting.Application.Budgets.Dashboard;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Budgets.v1;

public static class BudgetDashboardEndpoint
{
    internal static RouteHandlerBuilder MapBudgetDashboardEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}/dashboard", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetBudgetDashboardQuery(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(BudgetDashboardEndpoint))
            .WithSummary("get budget dashboard analytics")
            .WithDescription("get comprehensive budget performance metrics and analytics including variance analysis, utilization trends, and account breakdowns")
            .Produces<BudgetDashboardResponse>()
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
