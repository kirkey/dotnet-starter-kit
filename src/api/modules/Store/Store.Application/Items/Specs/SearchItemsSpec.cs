using FSH.Starter.WebApi.Store.Application.Items.Search.v1;

namespace FSH.Starter.WebApi.Store.Application.Items.Specs;

/// <summary>
/// Specification for searching items with various filters and pagination support.
/// Inherits from <see cref="EntitiesByPaginationFilterSpec{T,TResult}"/> to automatically handle pagination (Skip/Take).
/// </summary>
public sealed class SearchItemsSpec : EntitiesByPaginationFilterSpec<Item, ItemResponse>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchItemsSpec"/> class.
    /// </summary>
    /// <param name="request">The search items command containing filter criteria and pagination parameters.</param>
    public SearchItemsSpec(SearchItemsCommand request)
        : base(request)
    {
        Query
            .Where(i => i.Name.Contains(request.SearchTerm!), !string.IsNullOrWhiteSpace(request.SearchTerm))
            .Where(i => i.Sku == request.Sku, !string.IsNullOrWhiteSpace(request.Sku))
            .Where(i => i.Barcode == request.Barcode, !string.IsNullOrWhiteSpace(request.Barcode))
            .Where(i => i.CategoryId == request.CategoryId, request.CategoryId.HasValue)
            .Where(i => i.SupplierId == request.SupplierId, request.SupplierId.HasValue)
            .Where(i => i.IsPerishable == request.IsPerishable!.Value, request.IsPerishable.HasValue)
            .Where(i => i.IsSerialTracked == request.IsSerialTracked!.Value, request.IsSerialTracked.HasValue)
            .Where(i => i.IsLotTracked == request.IsLotTracked!.Value, request.IsLotTracked.HasValue)
            .Where(i => i.Brand != null && i.Brand.Contains(request.Brand!), !string.IsNullOrWhiteSpace(request.Brand))
            .Where(i => i.Manufacturer != null && i.Manufacturer.Contains(request.Manufacturer!), !string.IsNullOrWhiteSpace(request.Manufacturer))
            .OrderBy(i => i.Name, !request.HasOrderBy());
    }
}
