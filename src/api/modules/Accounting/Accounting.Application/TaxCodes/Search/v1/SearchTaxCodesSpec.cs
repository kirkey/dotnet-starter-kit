using Accounting.Application.TaxCodes.Responses;
using Accounting.Domain.Entities;

namespace Accounting.Application.TaxCodes.Search.v1;

public class SearchTaxCodesSpec : Specification<TaxCode, TaxCodeResponse>
{
    public SearchTaxCodesSpec(SearchTaxCodesCommand request)
    {
        Query
            .Where(t => string.IsNullOrEmpty(request.Code) || t.Code.Contains(request.Code))
            .Where(t => string.IsNullOrEmpty(request.TaxType) || t.TaxType.ToString() == request.TaxType)
            .Where(t => string.IsNullOrEmpty(request.Jurisdiction) || t.Jurisdiction!.Contains(request.Jurisdiction))
            .Where(t => !request.IsActive.HasValue || t.IsActive == request.IsActive.Value)
            .OrderBy(t => t.Code)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize);
    }
}
