using Accounting.Application.Vendors.Dashboard;
using Shared.Authorization;

namespace Accounting.Infrastructure.Endpoints.Vendors.v1;

public static class VendorDashboardEndpoint
{
    internal static RouteHandlerBuilder MapVendorDashboardEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/by-id/{id}/dashboard", async (DefaultIdType id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetVendorDashboardQuery(id)).ConfigureAwait(false);
                return Results.Ok(response);
            })
            .WithName(nameof(VendorDashboardEndpoint))
            .WithSummary("Get vendor dashboard analytics")
            .WithDescription("Retrieves comprehensive dashboard data including financial metrics, bills, payments, and trends for a specific vendor")
            .Produces<VendorDashboardResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.View, FshResources.Accounting))
            .MapToApiVersion(1);
    }
}
