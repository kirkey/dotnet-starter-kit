using FSH.Starter.WebApi.Store.Application.Bins.Search.v1;

namespace FSH.Starter.WebApi.Store.Application.Bins.Specs;

/// <summary>
/// Specification for searching bins with various filters.
/// </summary>
public sealed class SearchBinsSpec : Specification<Bin>
{
    public SearchBinsSpec(SearchBinsCommand request)
    {
        Query
            .Where(b => b.Name.Contains(request.SearchTerm!), !string.IsNullOrWhiteSpace(request.SearchTerm))
            .Where(b => b.WarehouseLocationId == request.WarehouseLocationId, request.WarehouseLocationId.HasValue)
            .Where(b => b.IsActive == request.IsActive, request.IsActive.HasValue);
    }
}
