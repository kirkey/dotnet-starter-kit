using FSH.Starter.WebApi.Store.Application.ItemSuppliers.Search.v1;
using ItemSupplierResponse = FSH.Starter.WebApi.Store.Application.ItemSuppliers.Get.v1.ItemSupplierResponse;

namespace FSH.Starter.WebApi.Store.Application.ItemSuppliers.Specs;

/// <summary>
/// Specification for searching item-supplier relationships with filters and pagination.
/// </summary>
public sealed class SearchItemSuppliersSpec : EntitiesByPaginationFilterSpec<ItemSupplier, ItemSupplierResponse>
{
    public SearchItemSuppliersSpec(SearchItemSuppliersCommand request)
        : base(request)
    {
        Query
            .Where(i => i.ItemId == request.ItemId, request.ItemId.HasValue)
            .Where(i => i.SupplierId == request.SupplierId, request.SupplierId.HasValue)
            .Where(i => i.IsActive == request.IsActive, request.IsActive.HasValue)
            .Where(i => i.IsPreferred == request.IsPreferred, request.IsPreferred.HasValue)
            .Where(i => i.CurrencyCode == request.CurrencyCode, !string.IsNullOrWhiteSpace(request.CurrencyCode))
            .Where(i => i.ReliabilityRating >= request.MinReliabilityRating, request.MinReliabilityRating.HasValue);

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
