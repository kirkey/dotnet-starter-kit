using FSH.Starter.WebApi.Store.Application.ItemSuppliers.Get.v1;

namespace FSH.Starter.WebApi.Store.Application.ItemSuppliers.Specs;

/// <summary>
/// Specification to get an item-supplier relationship by ID with response mapping.
/// </summary>
public sealed class GetItemSupplierByIdSpec : Specification<ItemSupplier, ItemSupplierResponse>
{
    public GetItemSupplierByIdSpec(DefaultIdType id)
    {
        Query.Where(i => i.Id == id);
    }
}
