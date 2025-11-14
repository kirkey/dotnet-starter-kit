namespace FSH.Starter.WebApi.HumanResources.Application.Benefits.Specifications;

/// <summary>
/// Specification for getting a benefit by ID.
/// </summary>
public class BenefitByIdSpec : Specification<Benefit>, ISingleResultSpecification<Benefit>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BenefitByIdSpec"/> class.
    /// </summary>
    public BenefitByIdSpec(DefaultIdType id)
    {
        Query.Where(x => x.Id == id);
    }
}

/// <summary>
/// Specification for searching benefits with filters.
/// </summary>
public class SearchBenefitsSpec : Specification<Benefit>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchBenefitsSpec"/> class.
    /// </summary>
    public SearchBenefitsSpec(Search.v1.SearchBenefitsRequest request)
    {
        Query.OrderBy(x => x.BenefitName);

        if (!string.IsNullOrWhiteSpace(request.SearchString))
            Query.Where(x => x.BenefitName.Contains(request.SearchString));

        if (!string.IsNullOrWhiteSpace(request.BenefitType))
            Query.Where(x => x.BenefitType == request.BenefitType);

        if (request.IsActive.HasValue)
            Query.Where(x => x.IsActive == request.IsActive);

        if (request.IsRequired.HasValue)
            Query.Where(x => x.IsRequired == request.IsRequired);
    }
}

