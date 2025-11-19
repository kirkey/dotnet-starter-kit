namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Specifications;

using Ardalis.Specification;
using Search.v1;
using Domain.Entities;

/// <summary>
/// Specification for searching tax brackets with filters.
/// </summary>
public sealed class SearchTaxBracketsSpec : Specification<TaxBracket>
{
    public SearchTaxBracketsSpec(SearchTaxBracketsRequest request)
    {
        Query.OrderBy(x => x.Year)
            .ThenBy(x => x.TaxType)
            .ThenBy(x => x.MinIncome);

        if (!string.IsNullOrWhiteSpace(request.TaxType))
            Query.Where(x => x.TaxType == request.TaxType);

        if (request.Year.HasValue)
            Query.Where(x => x.Year == request.Year);

        if (request.MinIncomeFrom.HasValue)
            Query.Where(x => x.MinIncome >= request.MinIncomeFrom);

        if (request.MinIncomeTo.HasValue)
            Query.Where(x => x.MinIncome <= request.MinIncomeTo);

        if (!string.IsNullOrWhiteSpace(request.FilingStatus))
            Query.Where(x => x.FilingStatus == request.FilingStatus);

        // Pagination
        Query.Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize);
    }
}

// This file has been reorganized. The class SearchTaxesSpec is now in its own file.
