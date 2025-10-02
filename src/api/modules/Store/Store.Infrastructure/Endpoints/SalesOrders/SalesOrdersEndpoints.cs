namespace Store.Infrastructure.Endpoints.SalesOrders;

/// <summary>
/// Endpoint configuration for Sales Orders module.
/// </summary>
public static class SalesOrdersEndpoints
{
    /// <summary>
    /// Maps all Sales Orders endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapSalesOrdersEndpoints(this IEndpointRouteBuilder app)
    {
        var salesOrdersGroup = app.MapGroup("/sales-orders")
            .WithTags("Sales-Orders")
            .WithDescription("Endpoints for managing sales orders");

        // Version 1 endpoints will be added here when implemented
        // salesOrdersGroup.MapCreateSalesOrderEndpoint();
        // salesOrdersGroup.MapUpdateSalesOrderEndpoint();
        // salesOrdersGroup.MapDeleteSalesOrderEndpoint();
        // salesOrdersGroup.MapGetSalesOrderEndpoint();
        // salesOrdersGroup.MapSearchSalesOrdersEndpoint();

        return app;
    }
}
