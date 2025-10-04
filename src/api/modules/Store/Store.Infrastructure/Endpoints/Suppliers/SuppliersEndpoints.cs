using Store.Infrastructure.Endpoints.Suppliers.v1;

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

        // Version 1 endpoints
        suppliersGroup.MapCreateSupplierEndpoint();
        suppliersGroup.MapUpdateSupplierEndpoint();
        suppliersGroup.MapDeleteSupplierEndpoint();
        suppliersGroup.MapGetSupplierEndpoint();
        suppliersGroup.MapSearchSuppliersEndpoint();
        suppliersGroup.MapActivateSupplierEndpoint();
        suppliersGroup.MapDeactivateSupplierEndpoint();

        return app;
    }
}
