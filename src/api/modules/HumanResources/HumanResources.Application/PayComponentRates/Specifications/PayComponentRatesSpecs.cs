namespace FSH.Starter.WebApi.HumanResources.Application.PayComponentRates.Specifications;

/// <summary>
/// Specification for searching pay component rates with filters.
/// </summary>
public sealed class SearchPayComponentRatesSpec : Specification<PayComponentRate>
{
    public SearchPayComponentRatesSpec(Search.v1.SearchPayComponentRatesRequest request)
    {
        Query
            .Include(x => x.PayComponent)
            .OrderBy(x => x.PayComponentId)
            .ThenBy(x => x.Year)
            .ThenBy(x => x.MinAmount);

        if (request.PayComponentId.HasValue)
            Query.Where(x => x.PayComponentId == request.PayComponentId);

        if (request.Year.HasValue)
            Query.Where(x => x.Year == request.Year);

        if (request.MinAmountFrom.HasValue)
            Query.Where(x => x.MaxAmount >= request.MinAmountFrom);

        if (request.MaxAmountTo.HasValue)
            Query.Where(x => x.MinAmount <= request.MaxAmountTo);

        if (request.IsActive.HasValue)
            Query.Where(x => x.IsActive == request.IsActive);
    }
}

/// <summary>
/// Specification for getting a pay component rate by ID with includes.
/// </summary>
public sealed class PayComponentRateByIdSpec : Specification<PayComponentRate>, ISingleResultSpecification<PayComponentRate>
{
    public PayComponentRateByIdSpec(DefaultIdType id)
    {
        Query
            .Where(x => x.Id == id)
            .Include(x => x.PayComponent);
    }
}

