using Store.Infrastructure.Endpoints.ItemSuppliers.v1;

namespace Store.Infrastructure.Endpoints.ItemSuppliers;

/// <summary>
/// Carter module for ItemSupplier endpoints.
/// Routes all requests to separate versioned endpoint handlers.
/// </summary>
public class ItemSuppliersEndpoints() : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("store/item-suppliers").WithTags("item-suppliers");

        // Call the individual endpoint handler methods from v1
        group.MapCreateItemSupplierEndpoint();
        group.MapUpdateItemSupplierEndpoint();
        group.MapDeleteItemSupplierEndpoint();
        group.MapGetItemSupplierEndpoint();
        group.MapSearchItemSuppliersEndpoint();
    }
}
