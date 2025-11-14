namespace FSH.Starter.WebApi.HumanResources.Application.PerformanceReviews.Create.v1;

/// <summary>
/// Command to create a new performance review.
/// </summary>
public sealed record CreatePerformanceReviewCommand(
    DefaultIdType EmployeeId,
    DefaultIdType ReviewerId,
    DateTime ReviewPeriodStart,
    DateTime ReviewPeriodEnd,
    [property: DefaultValue("Annual")] string ReviewType = "Annual",
    [property: DefaultValue(0)] decimal OverallRating = 0,
    [property: DefaultValue(null)] string? Strengths = null,
    [property: DefaultValue(null)] string? AreasForImprovement = null,
    [property: DefaultValue(null)] string? Goals = null,
    [property: DefaultValue(null)] string? ReviewerComments = null,
    [property: DefaultValue(null)] string? EmployeeComments = null
) : IRequest<CreatePerformanceReviewResponse>;

/// <summary>
/// Response for performance review creation.
/// </summary>
public sealed record CreatePerformanceReviewResponse(
    DefaultIdType Id,
    DefaultIdType EmployeeId,
    DefaultIdType ReviewerId,
    DateTime ReviewPeriodStart,
    DateTime ReviewPeriodEnd,
    string ReviewType,
    string Status);

