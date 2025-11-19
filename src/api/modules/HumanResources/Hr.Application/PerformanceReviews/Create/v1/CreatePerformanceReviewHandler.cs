namespace FSH.Starter.WebApi.HumanResources.Application.PerformanceReviews.Create.v1;

/// <summary>
/// Handler for creating performance review.
/// </summary>
public sealed class CreatePerformanceReviewHandler(
    ILogger<CreatePerformanceReviewHandler> logger,
    [FromKeyedServices("hr:performancereviews")] IRepository<PerformanceReview> repository,
    [FromKeyedServices("hr:employees")] IReadRepository<Employee> employeeRepository)
    : IRequestHandler<CreatePerformanceReviewCommand, CreatePerformanceReviewResponse>
{
    public async Task<CreatePerformanceReviewResponse> Handle(
        CreatePerformanceReviewCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Verify employee exists
        var employee = await employeeRepository.GetByIdAsync(request.EmployeeId, cancellationToken);
        if (employee is null)
            throw new EmployeeNotFoundException(request.EmployeeId);

        // Verify reviewer exists
        var reviewer = await employeeRepository.GetByIdAsync(request.ReviewerId, cancellationToken);
        if (reviewer is null)
            throw new EmployeeNotFoundException(request.ReviewerId);

        // Create review
        var review = PerformanceReview.Create(
            request.EmployeeId,
            request.ReviewerId,
            request.ReviewPeriodStart,
            request.ReviewPeriodEnd,
            request.ReviewType);

        // Set optional fields
        if (request.OverallRating > 0)
            review.SetRating(request.OverallRating);

        if (!string.IsNullOrWhiteSpace(request.Strengths))
            review.SetStrengths(request.Strengths);

        if (!string.IsNullOrWhiteSpace(request.AreasForImprovement))
            review.SetAreasForImprovement(request.AreasForImprovement);

        if (!string.IsNullOrWhiteSpace(request.Goals))
            review.SetGoals(request.Goals);

        if (!string.IsNullOrWhiteSpace(request.ReviewerComments))
            review.SetReviewerComments(request.ReviewerComments);

        if (!string.IsNullOrWhiteSpace(request.EmployeeComments))
            review.SetEmployeeComments(request.EmployeeComments);

        await repository.AddAsync(review, cancellationToken);

        logger.LogInformation(
            "Performance review created: Employee {EmployeeId}, Reviewer {ReviewerId}, Type {ReviewType}",
            review.EmployeeId,
            review.ReviewerId,
            review.ReviewType);

        return new CreatePerformanceReviewResponse(
            review.Id,
            review.EmployeeId,
            review.ReviewerId,
            review.ReviewPeriodStart,
            review.ReviewPeriodEnd,
            review.ReviewType,
            review.Status);
    }
}

