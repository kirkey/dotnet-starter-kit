namespace FSH.Starter.WebApi.HumanResources.Application.Taxes.Specifications;

/// <summary>
/// Specification for getting a tax bracket by ID.
/// </summary>
public class TaxByIdSpec : Specification<TaxBracket>, ISingleResultSpecification<TaxBracket>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TaxByIdSpec"/> class.
    /// </summary>
    public TaxByIdSpec(DefaultIdType id)
    {
        Query.Where(x => x.Id == id);
    }
}

/// <summary>
/// Specification for searching tax brackets with filters.
/// </summary>
public class SearchTaxesSpec : Specification<TaxBracket>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchTaxesSpec"/> class.
    /// </summary>
    public SearchTaxesSpec(Search.v1.SearchTaxesRequest request)
    {
        Query.OrderBy(x => x.Year)
            .ThenBy(x => x.TaxType)
            .ThenBy(x => x.MinIncome);

        if (!string.IsNullOrWhiteSpace(request.TaxType))
            Query.Where(x => x.TaxType == request.TaxType);

        if (request.Year.HasValue)
            Query.Where(x => x.Year == request.Year);

        if (!string.IsNullOrWhiteSpace(request.FilingStatus))
            Query.Where(x => x.FilingStatus == request.FilingStatus);

        if (request.MinIncomeFilter.HasValue)
            Query.Where(x => x.MaxIncome >= request.MinIncomeFilter);

        if (request.MaxIncomeFilter.HasValue)
            Query.Where(x => x.MinIncome <= request.MaxIncomeFilter);
    }
}

