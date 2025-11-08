using Accounting.Infrastructure.Endpoints.InventoryItems.v1;

namespace Accounting.Infrastructure.Endpoints.InventoryItems;

public static class InventoryItemsEndpoints
{
    internal static IEndpointRouteBuilder MapInventoryItemsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/inventory-items")
            .WithTags("Inventory Items")
            .WithDescription("Endpoints for managing inventory items")
            .MapToApiVersion(1);

        // CRUD operations
        group.MapInventoryItemCreateEndpoint();
        group.MapInventoryItemGetEndpoint();
        group.MapInventoryItemUpdateEndpoint();
        group.MapInventoryItemSearchEndpoint();

        // Workflow operations
        group.MapInventoryItemAddStockEndpoint();
        group.MapInventoryItemReduceStockEndpoint();
        group.MapInventoryItemDeactivateEndpoint();

        return app;
    }
}
