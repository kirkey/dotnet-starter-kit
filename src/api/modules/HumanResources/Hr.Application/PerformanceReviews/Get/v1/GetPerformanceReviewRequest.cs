namespace FSH.Starter.WebApi.HumanResources.Application.PerformanceReviews.Get.v1;

/// <summary>
/// Request to get performance review details.
/// </summary>
public sealed record GetPerformanceReviewRequest(DefaultIdType Id) : IRequest<PerformanceReviewResponse>;

/// <summary>
/// Response with performance review details.
/// </summary>
public sealed record PerformanceReviewResponse(
    DefaultIdType Id,
    DefaultIdType EmployeeId,
    string EmployeeName,
    DefaultIdType ReviewerId,
    string ReviewerName,
    DateTime ReviewPeriodStart,
    DateTime ReviewPeriodEnd,
    string ReviewType,
    string Status,
    decimal OverallRating,
    string? Strengths,
    string? AreasForImprovement,
    string? Goals,
    string? ReviewerComments,
    string? EmployeeComments,
    DateTime? SubmittedDate,
    DateTime? CompletedDate,
    DateTime? AcknowledgedDate);

