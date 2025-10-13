using FSH.Starter.WebApi.Store.Application.Bins.Search.v1;

namespace FSH.Starter.WebApi.Store.Application.Bins.Specs;

/// <summary>
/// Specification for searching bins with various filters and pagination support.
/// </summary>
public sealed class SearchBinsSpec : EntitiesByPaginationFilterSpec<Bin>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchBinsSpec"/> class.
    /// </summary>
    /// <param name="request">The search bins command containing filter criteria and pagination parameters.</param>
    public SearchBinsSpec(SearchBinsCommand request)
        : base(request)
    {
        Query
            .Where(b => b.Name.Contains(request.SearchTerm!), !string.IsNullOrWhiteSpace(request.SearchTerm))
            .Where(b => b.WarehouseLocationId == request.WarehouseLocationId, request.WarehouseLocationId.HasValue)
            .Where(b => b.IsActive == request.IsActive!.Value, request.IsActive.HasValue)
            .OrderBy(b => b.Name, !request.HasOrderBy());
    }
}
