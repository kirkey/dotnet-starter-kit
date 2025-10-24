using FSH.Starter.WebApi.Store.Application.ItemSuppliers.Get.v1;

namespace FSH.Starter.WebApi.Store.Application.ItemSuppliers.Specs;

/// <summary>
/// Specification to get an item-supplier relationship by ID with response mapping.
/// </summary>
public sealed class GetItemSupplierByIdSpec : Specification<ItemSupplier, ItemSupplierResponse>
{
    public GetItemSupplierByIdSpec(DefaultIdType id)
    {
        Query
            .Where(i => i.Id == id)
            .Include(i => i.Item)
            .Include(i => i.Supplier);

        Query.Select(i => new ItemSupplierResponse(
            i.Id,
            i.Name,
            i.Description,
            i.Notes,
            i.ItemId,
            i.Item.Name,
            i.SupplierId,
            i.Supplier.Name,
            i.SupplierPartNumber,
            i.UnitCost,
            i.LeadTimeDays,
            i.MinimumOrderQuantity,
            i.PackagingQuantity,
            i.IsPreferred,
            i.IsActive,
            i.ReliabilityRating,
            i.LastPriceUpdate,
            i.CreatedOn,
            i.LastModifiedOn
        ));
    }
}
