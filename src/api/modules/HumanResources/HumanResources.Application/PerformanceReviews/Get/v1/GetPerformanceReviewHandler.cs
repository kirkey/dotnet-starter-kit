namespace FSH.Starter.WebApi.HumanResources.Application.PerformanceReviews.Get.v1;

using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.HumanResources.Application.PerformanceReviews.Specifications;
using FSH.Starter.WebApi.HumanResources.Domain.Entities;
using FSH.Starter.WebApi.HumanResources.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Handler for getting performance review details.
/// </summary>
public sealed class GetPerformanceReviewHandler(
    [FromKeyedServices("hr:performancereviews")] IReadRepository<PerformanceReview> repository)
    : IRequestHandler<GetPerformanceReviewRequest, PerformanceReviewResponse>
{
    public async Task<PerformanceReviewResponse> Handle(
        GetPerformanceReviewRequest request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new PerformanceReviewByIdSpec(request.Id);
        var review = await repository.FirstOrDefaultAsync(spec, cancellationToken);

        if (review is null)
            throw new PerformanceReviewNotFoundException(request.Id);

        return new PerformanceReviewResponse(
            review.Id,
            review.EmployeeId,
            review.Employee.Name,
            review.ReviewerId,
            review.Reviewer.Name,
            review.ReviewPeriodStart,
            review.ReviewPeriodEnd,
            review.ReviewType,
            review.Status,
            review.OverallRating,
            review.Strengths,
            review.AreasForImprovement,
            review.Goals,
            review.ReviewerComments,
            review.EmployeeComments,
            review.SubmittedDate,
            review.CompletedDate,
            review.AcknowledgedDate);
    }
}

