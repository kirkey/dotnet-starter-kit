using FSH.Starter.WebApi.Store.Application.Items.Dashboard;

namespace Store.Infrastructure.Endpoints.Items.v1;

public static class GetItemDashboardEndpoint
{
    internal static RouteHandlerBuilder MapGetItemDashboardEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id}/dashboard", async (DefaultIdType id, ISender sender) =>
        {
            var response = await sender.Send(new GetItemDashboardQuery(id));
            return Results.Ok(response);
        })
        .WithName(nameof(GetItemDashboardEndpoint))
        .WithSummary("Get comprehensive dashboard analytics for a specific item")
        .WithDescription("Returns detailed performance metrics including sales trends, stock levels, movement classification, and supplier performance for the specified item.")
        .Produces<ItemDashboardResponse>()
        .RequirePermission("Permissions.Items.View")
        .MapToApiVersion(1)
        .WithOpenApi();
    }
}
