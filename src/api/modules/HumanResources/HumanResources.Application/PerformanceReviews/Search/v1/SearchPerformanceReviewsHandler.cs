namespace FSH.Starter.WebApi.HumanResources.Application.PerformanceReviews.Search.v1;

using FSH.Framework.Core.Paging;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.HumanResources.Application.PerformanceReviews.Specifications;
using FSH.Starter.WebApi.HumanResources.Domain.Entities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Handler for searching performance reviews.
/// </summary>
public sealed class SearchPerformanceReviewsHandler(
    [FromKeyedServices("hr:performancereviews")] IReadRepository<PerformanceReview> repository)
    : IRequestHandler<SearchPerformanceReviewsRequest, PagedList<PerformanceReviewDto>>
{
    public async Task<PagedList<PerformanceReviewDto>> Handle(
        SearchPerformanceReviewsRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchPerformanceReviewsSpec(request);
        var items = await repository.ListAsync(spec, cancellationToken);
        var totalCount = await repository.CountAsync(spec, cancellationToken);

        var dtos = items.Select(r => new PerformanceReviewDto(
            r.Id,
            r.EmployeeId,
            r.Employee.Name,
            r.ReviewerId,
            r.Reviewer.Name,
            r.ReviewPeriodEnd,
            r.ReviewType,
            r.Status,
            r.OverallRating)).ToList();

        return new PagedList<PerformanceReviewDto>(
            dtos,
            request.PageNumber,
            request.PageSize,
            totalCount);
    }
}

