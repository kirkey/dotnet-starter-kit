namespace FSH.Starter.WebApi.HumanResources.Application.PerformanceReviews.Specifications;

using Ardalis.Specification;
using FSH.Starter.WebApi.HumanResources.Domain.Entities;

/// <summary>
/// Specification for getting performance review by ID.
/// </summary>
public sealed class PerformanceReviewByIdSpec : Specification<PerformanceReview>, ISingleResultSpecification<PerformanceReview>
{
    public PerformanceReviewByIdSpec(DefaultIdType id)
    {
        Query.Where(x => x.Id == id)
            .Include(x => x.Employee)
            .Include(x => x.Reviewer);
    }
}

