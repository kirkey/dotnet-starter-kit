using Store.Infrastructure.Endpoints.InventoryTransactions.v1;

namespace Store.Infrastructure.Endpoints.InventoryTransactions;

/// <summary>
/// Endpoint configuration for Inventory Transactions module.
/// </summary>
public static class InventoryTransactionsEndpoints
{
    /// <summary>
    /// Maps all Inventory Transactions endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapInventoryTransactionsEndpoints(this IEndpointRouteBuilder app)
    {
        var transactionsGroup = app.MapGroup("/inventorytransactions")
            .WithTags("InventoryTransactions")
            .WithDescription("Endpoints for managing inventory transactions");

        // Version 1 endpoints
        transactionsGroup.MapCreateInventoryTransactionEndpoint();
        transactionsGroup.MapApproveInventoryTransactionEndpoint();
        transactionsGroup.MapDeleteInventoryTransactionEndpoint();
        transactionsGroup.MapGetInventoryTransactionEndpoint();
        transactionsGroup.MapSearchInventoryTransactionsEndpoint();

        return app;
    }
}
