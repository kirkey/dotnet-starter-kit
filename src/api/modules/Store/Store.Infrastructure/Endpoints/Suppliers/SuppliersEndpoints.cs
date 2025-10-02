namespace Store.Infrastructure.Endpoints.Suppliers;

/// <summary>
/// Endpoint configuration for Suppliers module.
/// </summary>
public static class SuppliersEndpoints
{
    /// <summary>
    /// Maps all Suppliers endpoints to the route builder.
    /// </summary>
    internal static IEndpointRouteBuilder MapSuppliersEndpoints(this IEndpointRouteBuilder app)
    {
        var suppliersGroup = app.MapGroup("/suppliers")
            .WithTags("Suppliers")
            .WithDescription("Endpoints for managing suppliers");

        // Version 1 endpoints will be added here when implemented
        // suppliersGroup.MapCreateSupplierEndpoint();
        // suppliersGroup.MapUpdateSupplierEndpoint();
        // suppliersGroup.MapDeleteSupplierEndpoint();
        // suppliersGroup.MapGetSupplierEndpoint();
        // suppliersGroup.MapSearchSuppliersEndpoint();

        return app;
    }
}
