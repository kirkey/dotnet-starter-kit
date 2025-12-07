using Store.Infrastructure.Endpoints.InventoryTransfers.v1;
using Carter;

namespace Store.Infrastructure.Endpoints.InventoryTransfers;

/// <summary>
/// Endpoint configuration for Inventory Transfers module.
/// Provides REST API endpoints for managing inventory transfers between warehouses.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class InventoryTransfersEndpoints() : CarterModule
{
    /// <summary>
    /// Maps all Inventory Transfers endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, Search, and business operation endpoints.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("store/inventory-transfers").WithTags("inventory-transfers");

        group.MapCreateInventoryTransferEndpoint();
        group.MapUpdateInventoryTransferEndpoint();
        group.MapDeleteInventoryTransferEndpoint();
        group.MapGetInventoryTransferEndpoint();
        group.MapSearchInventoryTransfersEndpoint();
        group.MapAddInventoryTransferItemEndpoint();
        group.MapRemoveInventoryTransferItemEndpoint();
        group.MapUpdateInventoryTransferItemEndpoint();
        group.MapApproveInventoryTransferEndpoint();
        group.MapMarkInTransitInventoryTransferEndpoint();
        group.MapCompleteInventoryTransferEndpoint();
        group.MapCancelInventoryTransferEndpoint();
    }
}
