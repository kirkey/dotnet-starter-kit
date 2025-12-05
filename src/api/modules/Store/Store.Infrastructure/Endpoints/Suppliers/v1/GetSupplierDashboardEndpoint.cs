using FSH.Starter.WebApi.Store.Application.Suppliers.Dashboard;

namespace Store.Infrastructure.Endpoints.Suppliers.v1;

public static class GetSupplierDashboardEndpoint
{
    internal static RouteHandlerBuilder MapGetSupplierDashboardEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id}/dashboard", async (DefaultIdType id, ISender sender) =>
        {
            var response = await sender.Send(new GetSupplierDashboardQuery(id));
            return Results.Ok(response);
        })
        .WithName(nameof(GetSupplierDashboardEndpoint))
        .WithSummary("Get comprehensive dashboard analytics for a specific supplier")
        .WithDescription("Returns detailed performance metrics including order trends, delivery performance, quality metrics, and comparative rankings for the specified supplier.")
        .Produces<SupplierDashboardResponse>()
        .RequirePermission("Permissions.Suppliers.View")
        .MapToApiVersion(1)
        .WithOpenApi();
    }
}
