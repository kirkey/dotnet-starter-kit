namespace FSH.Starter.WebApi.HumanResources.Application.Benefits.Specifications;

using Ardalis.Specification;
using Search.v1;
using Domain.Entities;

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

