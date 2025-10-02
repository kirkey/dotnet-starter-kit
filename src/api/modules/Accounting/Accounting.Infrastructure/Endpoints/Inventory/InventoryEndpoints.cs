using Accounting.Infrastructure.Endpoints.Inventory.v1;

namespace Accounting.Infrastructure.Endpoints.Inventory;

/// <summary>
/// Endpoint configuration for Inventory module.
/// </summary>
public static class InventoryEndpoints
{
    /// <summary>
    /// Maps all Inventory endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapInventoryEndpoints(this IEndpointRouteBuilder app)
    {
        var inventoryGroup = app.MapGroup("/inventory")
            .WithTags("Inventory")
            .WithDescription("Endpoints for managing inventory items");

        // Version 1 endpoints
        inventoryGroup.MapCreateInventoryItemEndpoint();

        return app;
    }
}
