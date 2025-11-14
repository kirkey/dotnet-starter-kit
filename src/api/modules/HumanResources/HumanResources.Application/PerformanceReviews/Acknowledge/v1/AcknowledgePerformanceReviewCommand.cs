namespace FSH.Starter.WebApi.HumanResources.Application.PerformanceReviews.Acknowledge.v1;

/// <summary>
/// Command for employee to acknowledge performance review.
/// </summary>
public sealed record AcknowledgePerformanceReviewCommand(
    DefaultIdType Id
) : IRequest<AcknowledgePerformanceReviewResponse>;

/// <summary>
/// Response for performance review acknowledgement.
/// </summary>
public sealed record AcknowledgePerformanceReviewResponse(
    DefaultIdType Id,
    string Status,
    DateTime AcknowledgedDate);

