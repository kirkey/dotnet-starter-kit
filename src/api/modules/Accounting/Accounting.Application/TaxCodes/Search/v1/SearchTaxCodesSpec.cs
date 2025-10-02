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

        Query.Select(t => new TaxCodeResponse
        {
            Id = t.Id,
            Code = t.Code,
            Name = t.Name,
            TaxType = t.TaxType.ToString(),
            Rate = t.Rate,
            IsCompound = t.IsCompound,
            Jurisdiction = t.Jurisdiction,
            EffectiveDate = t.EffectiveDate,
            ExpiryDate = t.ExpiryDate,
            IsActive = t.IsActive,
            TaxCollectedAccountId = t.TaxCollectedAccountId,
            TaxPaidAccountId = t.TaxPaidAccountId,
            TaxAuthority = t.TaxAuthority,
            TaxRegistrationNumber = t.TaxRegistrationNumber,
            ReportingCategory = t.ReportingCategory,
            Description = t.Description,
            CreatedOn = t.CreatedOn,
            CreatedBy = t.CreatedBy,
            LastModifiedOn = t.LastModifiedOn,
            LastModifiedBy = t.LastModifiedBy
        });
    }
}
