namespace FSH.Starter.WebApi.HumanResources.Application.PerformanceReviews.Submit.v1;

/// <summary>
/// Command to submit performance review.
/// </summary>
public sealed record SubmitPerformanceReviewCommand(
    DefaultIdType Id
) : IRequest<SubmitPerformanceReviewResponse>;

/// <summary>
/// Response for performance review submission.
/// </summary>
public sealed record SubmitPerformanceReviewResponse(
    DefaultIdType Id,
    string Status,
    DateTime SubmittedDate);

