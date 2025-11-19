namespace FSH.Starter.WebApi.HumanResources.Application.PerformanceReviews.Update.v1;

/// <summary>
/// Handler for updating performance review.
/// </summary>
public sealed class UpdatePerformanceReviewHandler(
    ILogger<UpdatePerformanceReviewHandler> logger,
    [FromKeyedServices("hr:performancereviews")] IRepository<PerformanceReview> repository)
    : IRequestHandler<UpdatePerformanceReviewCommand, UpdatePerformanceReviewResponse>
{
    public async Task<UpdatePerformanceReviewResponse> Handle(
        UpdatePerformanceReviewCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var review = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (review is null)
            throw new PerformanceReviewNotFoundException(request.Id);

        // Update fields if provided
        if (request.OverallRating is > 0)
            review.SetRating(request.OverallRating.Value);

        if (request.Strengths != null)
            review.SetStrengths(request.Strengths);

        if (request.AreasForImprovement != null)
            review.SetAreasForImprovement(request.AreasForImprovement);

        if (request.Goals != null)
            review.SetGoals(request.Goals);

        if (request.ReviewerComments != null)
            review.SetReviewerComments(request.ReviewerComments);

        if (request.EmployeeComments != null)
            review.SetEmployeeComments(request.EmployeeComments);

        await repository.UpdateAsync(review, cancellationToken);

        logger.LogInformation("Performance review {Id} updated", review.Id);

        return new UpdatePerformanceReviewResponse(
            review.Id,
            review.OverallRating,
            review.Status);
    }
}

