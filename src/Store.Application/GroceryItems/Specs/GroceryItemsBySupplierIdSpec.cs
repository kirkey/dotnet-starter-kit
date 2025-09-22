namespace FSH.Starter.WebApi.Store.Application.GroceryItems.Specs;

/// <summary>
/// Specification that filters grocery items by SupplierId.
/// </summary>
public sealed class GroceryItemsBySupplierIdSpec : Specification<GroceryItem>
{
    public GroceryItemsBySupplierIdSpec(DefaultIdType supplierId)
    {
        Query.Where(i => i.SupplierId == supplierId);
    }
}

