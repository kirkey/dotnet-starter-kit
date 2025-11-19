namespace FSH.Starter.WebApi.HumanResources.Application.TaxBrackets.Specifications;

/// <summary>
/// Specification for searching tax brackets with filters.
/// </summary>
public sealed class SearchTaxBracketsSpec : Specification<TaxBracket>
{
    public SearchTaxBracketsSpec(Search.v1.SearchTaxBracketsRequest request)
    {
        Query
            .OrderBy(x => x.Year)
            .ThenBy(x => x.TaxType)
            .ThenBy(x => x.MinIncome);

        if (!string.IsNullOrWhiteSpace(request.TaxType))
            Query.Where(x => x.TaxType == request.TaxType);

        if (request.Year.HasValue)
            Query.Where(x => x.Year == request.Year);

        if (!string.IsNullOrWhiteSpace(request.FilingStatus))
            Query.Where(x => x.FilingStatus == request.FilingStatus);

        if (request.IncomeFrom.HasValue)
            Query.Where(x => x.MaxIncome >= request.IncomeFrom);

        if (request.IncomeTo.HasValue)
            Query.Where(x => x.MinIncome <= request.IncomeTo);
    }
}

/// <summary>
/// Specification for getting a tax bracket by ID.
/// </summary>
public sealed class TaxBracketByIdSpec : Specification<TaxBracket>, ISingleResultSpecification<TaxBracket>
{
    public TaxBracketByIdSpec(DefaultIdType id)
    {
        Query.Where(x => x.Id == id);
    }
}

