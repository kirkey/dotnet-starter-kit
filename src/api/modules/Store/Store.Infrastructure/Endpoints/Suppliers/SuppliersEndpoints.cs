using Store.Infrastructure.Endpoints.Suppliers.v1;

namespace Store.Infrastructure.Endpoints.Suppliers;

/// <summary>
/// Carter module for Suppliers endpoints.
/// Routes all requests to separate versioned endpoint handlers.
/// </summary>
public class SuppliersEndpoints() : CarterModule("store")
{
    /// <summary>
    /// Maps all Suppliers endpoints to the route builder.
    /// </summary>
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("store/suppliers").WithTags("suppliers");

        // Call the individual endpoint handler methods from v1
        group.MapCreateSupplierEndpoint();
        group.MapUpdateSupplierEndpoint();
        group.MapDeleteSupplierEndpoint();
        group.MapGetSupplierEndpoint();
        group.MapSearchSuppliersEndpoint();
        group.MapActivateSupplierEndpoint();
        group.MapDeactivateSupplierEndpoint();
    }
}
