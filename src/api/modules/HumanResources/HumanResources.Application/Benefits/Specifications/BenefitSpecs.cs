namespace FSH.Starter.WebApi.HumanResources.Application.Benefits.Specifications;

using Ardalis.Specification;
using FSH.Starter.WebApi.HumanResources.Application.Benefits.Search.v1;
using FSH.Starter.WebApi.HumanResources.Domain.Entities;

/// <summary>
/// Specification for getting benefit by ID.
/// </summary>
public sealed class BenefitByIdSpec : Specification<Benefit>, ISingleResultSpecification<Benefit>
{
    public BenefitByIdSpec(DefaultIdType id)
    {
        Query.Where(x => x.Id == id);
    }
}

/// <summary>
/// Specification for searching benefits with filters.
/// </summary>
public sealed class SearchBenefitsSpec : Specification<Benefit>
{
    public SearchBenefitsSpec(SearchBenefitsRequest request)
    {
        Query.OrderBy(x => x.BenefitType)
            .ThenBy(x => x.BenefitName);

        if (!string.IsNullOrWhiteSpace(request.BenefitType))
            Query.Where(x => x.BenefitType == request.BenefitType);

        if (request.IsMandatory.HasValue)
            Query.Where(x => x.IsMandatory == request.IsMandatory.Value);

        if (request.IsActive.HasValue)
            Query.Where(x => x.IsActive == request.IsActive.Value);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            Query.Where(x => x.BenefitName.Contains(request.SearchTerm));

        // Pagination
        Query.Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize);
    }
}

/// <summary>
/// Specification for getting mandatory benefits.
/// </summary>
public sealed class MandatoryBenefitsSpec : Specification<Benefit>
{
    public MandatoryBenefitsSpec()
    {
        Query.Where(x => x.IsMandatory && x.IsActive)
            .OrderBy(x => x.BenefitName);
    }
}

/// <summary>
/// Specification for getting benefits by type.
/// </summary>
public sealed class BenefitsByTypeSpec : Specification<Benefit>
{
    public BenefitsByTypeSpec(string benefitType)
    {
        Query.Where(x => x.BenefitType == benefitType && x.IsActive)
            .OrderBy(x => x.BenefitName);
    }
}

