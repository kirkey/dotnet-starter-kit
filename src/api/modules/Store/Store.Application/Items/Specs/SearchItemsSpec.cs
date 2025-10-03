using FSH.Starter.WebApi.Store.Application.Items.Search.v1;

namespace FSH.Starter.WebApi.Store.Application.Items.Specs;

/// <summary>
/// Specification for searching items with various filters.
/// </summary>
public sealed class SearchItemsSpec : Specification<Item>
{
    public SearchItemsSpec(SearchItemsCommand request)
    {
        Query
            .Where(i => i.Name.Contains(request.SearchTerm!), !string.IsNullOrWhiteSpace(request.SearchTerm))
            .Where(i => i.CategoryId == request.CategoryId, request.CategoryId.HasValue)
            .Where(i => i.SupplierId == request.SupplierId, request.SupplierId.HasValue);
    }
}
