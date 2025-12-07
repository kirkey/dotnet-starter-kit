using Accounting.Infrastructure.Endpoints.InventoryItems.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.InventoryItems;

/// <summary>
/// Endpoint configuration for InventoryItems module.
/// Provides comprehensive REST API endpoints for managing inventory-items.
/// Uses the ICarterModule delegated pattern with extension methods for each operation.
/// </summary>
public class InventoryItemsEndpoints() : CarterModule
{
    /// <summary>
    /// Maps all InventoryItems endpoints to the route builder.
    /// Delegates to extension methods for Create, Read, Update, Delete, and business operation endpoints.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("accounting/inventory-items").WithTags("inventory-item");

        group.MapInventoryItemAddStockEndpoint();
        group.MapInventoryItemCreateEndpoint();
        group.MapInventoryItemDeactivateEndpoint();
        group.MapInventoryItemGetEndpoint();
        group.MapInventoryItemReduceStockEndpoint();
        group.MapInventoryItemSearchEndpoint();
        group.MapInventoryItemUpdateEndpoint();
    }
}
