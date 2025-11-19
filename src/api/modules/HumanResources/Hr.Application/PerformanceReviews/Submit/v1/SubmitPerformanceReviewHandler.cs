namespace FSH.Starter.WebApi.HumanResources.Application.PerformanceReviews.Submit.v1;

/// <summary>
/// Handler for submitting performance review.
/// </summary>
public sealed class SubmitPerformanceReviewHandler(
    ILogger<SubmitPerformanceReviewHandler> logger,
    [FromKeyedServices("hr:performancereviews")] IRepository<PerformanceReview> repository)
    : IRequestHandler<SubmitPerformanceReviewCommand, SubmitPerformanceReviewResponse>
{
    public async Task<SubmitPerformanceReviewResponse> Handle(
        SubmitPerformanceReviewCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var review = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (review is null)
            throw new PerformanceReviewNotFoundException(request.Id);

        review.Submit();

        await repository.UpdateAsync(review, cancellationToken);

        logger.LogInformation("Performance review {Id} submitted", review.Id);

        return new SubmitPerformanceReviewResponse(
            review.Id,
            review.Status,
            review.SubmittedDate!.Value);
    }
}

