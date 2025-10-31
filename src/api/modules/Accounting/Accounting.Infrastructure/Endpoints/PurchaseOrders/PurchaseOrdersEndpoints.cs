using Accounting.Infrastructure.Endpoints.PurchaseOrders.v1;

namespace Accounting.Infrastructure.Endpoints.PurchaseOrders;

public static class PurchaseOrdersEndpoints
{
    internal static IEndpointRouteBuilder MapPurchaseOrdersEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/purchase-orders")
            .WithTags("Purchase Orders")
            .WithDescription("Endpoints for managing purchase orders")
            .MapToApiVersion(1);

        group.MapPurchaseOrderCreateEndpoint();
        group.MapPurchaseOrderGetEndpoint();
        group.MapPurchaseOrderSearchEndpoint();

        return app;
    }
}

