namespace FSH.Starter.WebApi.HumanResources.Application.Deductions.Specifications;

/// <summary>
/// Specification for getting a deduction by ID.
/// </summary>
public class DeductionByIdSpec : Specification<PayComponent>, ISingleResultSpecification<PayComponent>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DeductionByIdSpec"/> class.
    /// </summary>
    public DeductionByIdSpec(DefaultIdType id)
    {
        Query.Where(x => x.Id == id);
    }
}

/// <summary>
/// Specification for searching deductions with filters.
/// </summary>
public class SearchDeductionsSpec : Specification<PayComponent>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchDeductionsSpec"/> class.
    /// </summary>
    public SearchDeductionsSpec(Search.v1.SearchDeductionsRequest request)
    {
        Query.OrderBy(x => x.ComponentName);

        if (!string.IsNullOrWhiteSpace(request.SearchString))
            Query.Where(x => x.ComponentName.Contains(request.SearchString));

        if (!string.IsNullOrWhiteSpace(request.ComponentType))
            Query.Where(x => x.ComponentType == request.ComponentType);

        if (request.IsActive.HasValue)
            Query.Where(x => x.IsActive == request.IsActive);

        if (request.IsCalculated.HasValue)
            Query.Where(x => x.IsCalculated == request.IsCalculated);
    }
}

