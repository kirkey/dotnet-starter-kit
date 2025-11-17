namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Specifications;

/// <summary>
/// Specification for searching tax master configurations with filters.
/// </summary>
public sealed class SearchTaxesSpec : EntitiesByPaginationFilterSpec<TaxMaster, Search.v1.TaxDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchTaxesSpec"/> class.
    /// </summary>
    public SearchTaxesSpec(Search.v1.SearchTaxesRequest request)
        : base(request) =>
        Query
            .OrderBy(x => x.Code, !request.HasOrderBy())
            .Where(x => x.Code.Contains(request.Code!), !string.IsNullOrWhiteSpace(request.Code))
            .Where(x => x.TaxType == request.TaxType, !string.IsNullOrWhiteSpace(request.TaxType))
            .Where(x => x.Jurisdiction == request.Jurisdiction, !string.IsNullOrWhiteSpace(request.Jurisdiction))
            .Where(x => x.IsActive == request.IsActive, request.IsActive.HasValue)
            .Where(x => x.IsCompound == request.IsCompound, request.IsCompound.HasValue);
}

