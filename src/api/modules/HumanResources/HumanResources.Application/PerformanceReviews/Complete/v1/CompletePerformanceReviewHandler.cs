namespace FSH.Starter.WebApi.HumanResources.Application.PerformanceReviews.Complete.v1;

/// <summary>
/// Handler for completing performance review.
/// </summary>
public sealed class CompletePerformanceReviewHandler(
    ILogger<CompletePerformanceReviewHandler> logger,
    [FromKeyedServices("hr:performancereviews")] IRepository<PerformanceReview> repository)
    : IRequestHandler<CompletePerformanceReviewCommand, CompletePerformanceReviewResponse>
{
    public async Task<CompletePerformanceReviewResponse> Handle(
        CompletePerformanceReviewCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var review = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (review is null)
            throw new PerformanceReviewNotFoundException(request.Id);

        review.Complete();

        await repository.UpdateAsync(review, cancellationToken);

        logger.LogInformation("Performance review {Id} completed", review.Id);

        return new CompletePerformanceReviewResponse(
            review.Id,
            review.Status,
            review.CompletedDate!.Value);
    }
}

