namespace FSH.Starter.WebApi.HumanResources.Application.PerformanceReviews.Specifications;

using Ardalis.Specification;
using FSH.Starter.WebApi.HumanResources.Domain.Entities;

/// <summary>
/// Specification for getting pending reviews (awaiting submission).
/// </summary>
public sealed class PendingReviewsSpec : Specification<PerformanceReview>
{
    public PendingReviewsSpec()
    {
        Query.Where(x => x.Status == ReviewStatus.Draft)
            .OrderBy(x => x.ReviewPeriodEnd)
            .Include(x => x.Employee)
            .Include(x => x.Reviewer);
    }
}

