namespace FSH.Starter.WebApi.HumanResources.Application.PerformanceReviews.Acknowledge.v1;

/// <summary>
/// Handler for acknowledging performance review.
/// </summary>
public sealed class AcknowledgePerformanceReviewHandler(
    ILogger<AcknowledgePerformanceReviewHandler> logger,
    [FromKeyedServices("hr:performancereviews")] IRepository<PerformanceReview> repository)
    : IRequestHandler<AcknowledgePerformanceReviewCommand, AcknowledgePerformanceReviewResponse>
{
    public async Task<AcknowledgePerformanceReviewResponse> Handle(
        AcknowledgePerformanceReviewCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var review = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (review is null)
            throw new PerformanceReviewNotFoundException(request.Id);

        review.Acknowledge();

        await repository.UpdateAsync(review, cancellationToken);

        logger.LogInformation("Performance review {Id} acknowledged by employee", review.Id);

        return new AcknowledgePerformanceReviewResponse(
            review.Id,
            review.Status,
            review.AcknowledgedDate!.Value);
    }
}

