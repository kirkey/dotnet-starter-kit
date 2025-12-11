using FSH.Starter.WebApi.Store.Application.Warehouses.Dashboard;

namespace Store.Infrastructure.Endpoints.Warehouses.v1;

public static class GetWarehouseDashboardEndpoint
{
    internal static RouteHandlerBuilder MapGetWarehouseDashboardEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id}/dashboard", async (DefaultIdType id, ISender sender) =>
        {
            var response = await sender.Send(new GetWarehouseDashboardQuery(id));
            return Results.Ok(response);
        })
        .WithName(nameof(GetWarehouseDashboardEndpoint))
        .WithSummary("Get comprehensive dashboard analytics for a specific warehouse")
        .WithDescription("Returns detailed performance metrics including capacity utilization, inventory summary, movement trends, operations metrics, and alerts for the specified warehouse.")
        .Produces<WarehouseDashboardResponse>()
        .RequirePermission("Permissions.Warehouses.View")
        .MapToApiVersion(1)
        .WithOpenApi();
    }
}
