using Store.Infrastructure.Endpoints.ItemSuppliers.v1;

namespace Store.Infrastructure.Endpoints.ItemSuppliers;

public static class ItemSuppliersEndpoints
{
    internal static IEndpointRouteBuilder MapItemSuppliersEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var itemSuppliersGroup = endpoints.MapGroup("/itemsuppliers")
            .WithTags("ItemSuppliers")
            .WithDescription("Endpoints for managing item-supplier relationships");

        itemSuppliersGroup.MapCreateItemSupplierEndpoint();
        itemSuppliersGroup.MapUpdateItemSupplierEndpoint();
        itemSuppliersGroup.MapDeleteItemSupplierEndpoint();
        itemSuppliersGroup.MapGetItemSupplierEndpoint();
        itemSuppliersGroup.MapSearchItemSuppliersEndpoint();

        return endpoints;
    }
}
