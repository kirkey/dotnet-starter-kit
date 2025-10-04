using Store.Infrastructure.Endpoints.InventoryTransfers.v1;

namespace Store.Infrastructure.Endpoints.InventoryTransfers;

/// <summary>
/// Endpoint configuration for Inventory Transfers module.
/// </summary>
public static class InventoryTransfersEndpoints
{
    /// <summary>
    /// Maps all Inventory Transfers endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapInventoryTransfersEndpoints(this IEndpointRouteBuilder app)
    {
        var inventoryTransfersGroup = app.MapGroup("/inventory-transfers")
            .WithTags("Inventory-Transfers")
            .WithDescription("Endpoints for managing inventory transfers between locations");

        // Version 1 endpoints
        inventoryTransfersGroup.MapCreateInventoryTransferEndpoint();
        inventoryTransfersGroup.MapUpdateInventoryTransferEndpoint();
        inventoryTransfersGroup.MapDeleteInventoryTransferEndpoint();
        inventoryTransfersGroup.MapGetInventoryTransferEndpoint();
        inventoryTransfersGroup.MapSearchInventoryTransfersEndpoint();
        inventoryTransfersGroup.MapAddInventoryTransferItemEndpoint();
        inventoryTransfersGroup.MapRemoveInventoryTransferItemEndpoint();
        inventoryTransfersGroup.MapUpdateInventoryTransferItemEndpoint();
        inventoryTransfersGroup.MapApproveInventoryTransferEndpoint();
        inventoryTransfersGroup.MapMarkInTransitInventoryTransferEndpoint();
        inventoryTransfersGroup.MapCompleteInventoryTransferEndpoint();
        inventoryTransfersGroup.MapCancelInventoryTransferEndpoint();

        return app;
    }
}
