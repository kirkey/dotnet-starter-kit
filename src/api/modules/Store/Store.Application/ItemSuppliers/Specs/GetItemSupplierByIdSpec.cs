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

        Query.Select(i => new ItemSupplierResponse(
            i.Id,
            i.ItemId,
            i.SupplierId,
            i.SupplierPartNumber,
            i.UnitCost,
            i.LeadTimeDays,
            i.MinimumOrderQuantity,
            i.PackagingQuantity,
            i.CurrencyCode,
            i.IsPreferred,
            i.IsActive,
            i.ReliabilityRating,
            i.LastPriceUpdate,
            i.CreatedOn,
            i.CreatedBy));
    }
}
