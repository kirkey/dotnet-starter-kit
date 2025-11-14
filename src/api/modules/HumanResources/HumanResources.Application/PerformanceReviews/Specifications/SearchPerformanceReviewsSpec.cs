namespace FSH.Starter.WebApi.HumanResources.Application.PerformanceReviews.Specifications;

using Ardalis.Specification;
using FSH.Starter.WebApi.HumanResources.Application.PerformanceReviews.Search.v1;
using FSH.Starter.WebApi.HumanResources.Domain.Entities;

/// <summary>
/// Specification for searching performance reviews with filters.
/// </summary>
public sealed class SearchPerformanceReviewsSpec : Specification<PerformanceReview>
{
    public SearchPerformanceReviewsSpec(SearchPerformanceReviewsRequest request)
    {
        Query.OrderByDescending(x => x.ReviewPeriodEnd)
            .Include(x => x.Employee)
            .Include(x => x.Reviewer);

        if (request.EmployeeId.HasValue)
            Query.Where(x => x.EmployeeId == request.EmployeeId);

        if (request.ReviewerId.HasValue)
            Query.Where(x => x.ReviewerId == request.ReviewerId);

        if (!string.IsNullOrWhiteSpace(request.Status))
            Query.Where(x => x.Status == request.Status);

        if (!string.IsNullOrWhiteSpace(request.ReviewType))
            Query.Where(x => x.ReviewType == request.ReviewType);

        if (request.PeriodStart.HasValue)
            Query.Where(x => x.ReviewPeriodStart >= request.PeriodStart.Value);

        if (request.PeriodEnd.HasValue)
            Query.Where(x => x.ReviewPeriodEnd <= request.PeriodEnd.Value);

        // Pagination
        Query.Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize);
    }
}

