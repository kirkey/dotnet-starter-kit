using Accounting.Application.ChartOfAccounts.Dashboard;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.ChartOfAccounts.v1;

public static class ChartOfAccountDashboardEndpoint
{
    internal static RouteHandlerBuilder MapChartOfAccountDashboardEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}/dashboard", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetChartOfAccountDashboardQuery(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(ChartOfAccountDashboardEndpoint))
            .WithSummary("Get chart of account dashboard analytics")
            .WithDescription("Retrieves comprehensive dashboard data including balance metrics, activity, transactions, and trends for a specific chart of account")
            .Produces<ChartOfAccountDashboardResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
