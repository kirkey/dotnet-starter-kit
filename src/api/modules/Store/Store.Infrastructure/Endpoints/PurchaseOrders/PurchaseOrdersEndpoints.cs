using Store.Infrastructure.Endpoints.PurchaseOrders.v1;

namespace Store.Infrastructure.Endpoints.PurchaseOrders;

/// <summary>
/// Endpoint configuration for Purchase Orders module.
/// </summary>
public static class PurchaseOrdersEndpoints
{
    /// <summary>
    /// Maps all Purchase Orders endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapPurchaseOrdersEndpoints(this IEndpointRouteBuilder app)
    {
        var purchaseOrdersGroup = app.MapGroup("/purchase-orders")
            .WithTags("Purchase-Orders")
            .WithDescription("Endpoints for managing purchase orders");

        // Version 1 endpoints
        purchaseOrdersGroup.MapReceivePurchaseOrderItemQuantityEndpoint();
        purchaseOrdersGroup.MapUpdatePurchaseOrderItemQuantityEndpoint();
        purchaseOrdersGroup.MapRemovePurchaseOrderItemEndpoint();

        return app;
    }
}
