using Accounting.Application.TaxCodes.Responses;

namespace Accounting.Application.TaxCodes.Search.v1;

/// <summary>
/// Specification for searching tax codes with various filters and pagination support.
/// </summary>
public class SearchTaxCodesSpec : EntitiesByPaginationFilterSpec<TaxCode, TaxCodeResponse>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchTaxCodesSpec"/> class.
    /// </summary>
    /// <param name="request">The search tax codes command containing filter criteria and pagination parameters.</param>
    public SearchTaxCodesSpec(SearchTaxCodesCommand request)
        : base(request)
    {
        Query
            .Where(t => t.Code.Contains(request.Code!), !string.IsNullOrEmpty(request.Code))
            .Where(t => t.TaxType.ToString() == request.TaxType, !string.IsNullOrEmpty(request.TaxType))
            .Where(t => t.Jurisdiction!.Contains(request.Jurisdiction!), !string.IsNullOrEmpty(request.Jurisdiction))
            .Where(t => t.IsActive == request.IsActive!.Value, request.IsActive.HasValue)
            .OrderBy(t => t.Code, !request.HasOrderBy());
    }
}
