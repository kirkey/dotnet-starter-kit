using FSH.Starter.WebApi.Store.Application.ItemSuppliers.Search.v1;

namespace FSH.Starter.WebApi.Store.Application.ItemSuppliers.Specs;

/// <summary>
/// Specification for searching item-supplier relationships with filters and pagination.
/// </summary>
public sealed class SearchItemSuppliersSpec : EntitiesByPaginationFilterSpec<ItemSupplier>
{
    public SearchItemSuppliersSpec(SearchItemSuppliersCommand request)
        : base(request)
    {
        Query
            .Include(i => i.Item)
            .Include(i => i.Supplier)
            .Where(i => i.ItemId == request.ItemId, request.ItemId.HasValue)
            .Where(i => i.SupplierId == request.SupplierId, request.SupplierId.HasValue)
            .Where(i => i.IsActive == request.IsActive, request.IsActive.HasValue)
            .Where(i => i.IsPreferred == request.IsPreferred, request.IsPreferred.HasValue)
            .Where(i => i.ReliabilityRating >= request.MinReliabilityRating, request.MinReliabilityRating.HasValue);
    }
}
