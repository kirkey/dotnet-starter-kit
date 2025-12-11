using Accounting.Application.Customers.Dashboard;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Customers.v1;

public static class CustomerDashboardEndpoint
{
    internal static RouteHandlerBuilder MapCustomerDashboardEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}/dashboard", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetCustomerDashboardQuery(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(CustomerDashboardEndpoint))
            .WithSummary("Get customer dashboard analytics")
            .WithDescription("Retrieves comprehensive dashboard data including credit, invoices, payments, aging, and trends for a specific customer")
            .Produces<CustomerDashboardResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
