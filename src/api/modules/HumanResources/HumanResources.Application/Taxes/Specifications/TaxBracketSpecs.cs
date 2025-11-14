namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Specifications;

using Ardalis.Specification;
using FSH.Starter.WebApi.HumanResources.Application.Taxes.Search.v1;

/// <summary>
/// Specification for getting tax bracket by ID.
/// </summary>
public sealed class TaxBracketByIdSpec : Specification<TaxBracket>, ISingleResultSpecification<TaxBracket>
{
    public TaxBracketByIdSpec(DefaultIdType id)
    {
        Query.Where(x => x.Id == id);
    }
}

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

/// <summary>
/// Specification for getting tax brackets by type and year.
/// Used for payroll tax calculation.
/// </summary>
public sealed class TaxBracketsByTypeAndYearSpec : Specification<TaxBracket>
{
    public TaxBracketsByTypeAndYearSpec(string taxType, int year)
    {
        Query.Where(x => x.TaxType == taxType && x.Year == year)
            .OrderBy(x => x.MinIncome);
    }
}

/// <summary>
/// Specification for getting applicable tax bracket for an income amount.
/// </summary>
public sealed class TaxBracketByIncomeSpec : Specification<TaxBracket>, ISingleResultSpecification<TaxBracket>
{
    public TaxBracketByIncomeSpec(string taxType, int year, decimal income, string? filingStatus = null)
    {
        Query.Where(x => x.TaxType == taxType &&
                        x.Year == year &&
                        x.MinIncome <= income &&
                        x.MaxIncome > income);

        if (!string.IsNullOrWhiteSpace(filingStatus))
            Query.Where(x => x.FilingStatus == filingStatus || x.FilingStatus == null);

        Query.OrderBy(x => x.MinIncome);
    }
}

