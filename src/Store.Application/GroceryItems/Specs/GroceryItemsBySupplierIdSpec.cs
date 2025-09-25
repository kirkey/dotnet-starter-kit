namespace FSH.Starter.WebApi.Store.Application.GroceryItems.Specs;

/// <summary>
/// Specification to filter grocery items by supplier ID.
/// Used for supplier-based filtering and reporting.
/// </summary>
public sealed class GroceryItemsBySupplierIdSpec : Specification<GroceryItem>
{
    public GroceryItemsBySupplierIdSpec(DefaultIdType supplierId)
    {
        Query.Where(item => item.SupplierId == supplierId);
    }
}
