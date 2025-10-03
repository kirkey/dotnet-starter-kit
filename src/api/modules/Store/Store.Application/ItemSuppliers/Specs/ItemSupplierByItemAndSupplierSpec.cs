namespace FSH.Starter.WebApi.Store.Application.ItemSuppliers.Specs;

/// <summary>
/// Specification to find an item-supplier relationship by item ID and supplier ID.
/// </summary>
public sealed class ItemSupplierByItemAndSupplierSpec : Specification<ItemSupplier>
{
    public ItemSupplierByItemAndSupplierSpec(DefaultIdType itemId, DefaultIdType supplierId)
    {
        Query.Where(i => i.ItemId == itemId && i.SupplierId == supplierId);
    }
}
