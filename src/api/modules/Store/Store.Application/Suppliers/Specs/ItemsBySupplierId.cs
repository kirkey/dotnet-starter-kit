namespace FSH.Starter.WebApi.Store.Application.Suppliers.Specs;

/// <summary>
/// Specification to find items by supplier ID.
/// </summary>
public sealed class ItemsBySupplierId : Specification<Item>
{
    public ItemsBySupplierId(DefaultIdType supplierId)
    {
        Query.Where(item => item.SupplierId == supplierId);
    }
}
