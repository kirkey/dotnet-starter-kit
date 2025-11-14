namespace FSH.Starter.WebApi.HumanResources.Application.PerformanceReviews.Complete.v1;

/// <summary>
/// Command to complete performance review.
/// </summary>
public sealed record CompletePerformanceReviewCommand(
    DefaultIdType Id
) : IRequest<CompletePerformanceReviewResponse>;

/// <summary>
/// Response for performance review completion.
/// </summary>
public sealed record CompletePerformanceReviewResponse(
    DefaultIdType Id,
    string Status,
    DateTime CompletedDate);

