namespace FSH.Starter.WebApi.HumanResources.Application.PerformanceReviews.Update.v1;

/// <summary>
/// Command to update performance review.
/// </summary>
public sealed record UpdatePerformanceReviewCommand(
    DefaultIdType Id,
    [property: DefaultValue(null)] decimal? OverallRating = null,
    [property: DefaultValue(null)] string? Strengths = null,
    [property: DefaultValue(null)] string? AreasForImprovement = null,
    [property: DefaultValue(null)] string? Goals = null,
    [property: DefaultValue(null)] string? ReviewerComments = null,
    [property: DefaultValue(null)] string? EmployeeComments = null
) : IRequest<UpdatePerformanceReviewResponse>;

/// <summary>
/// Response for performance review update.
/// </summary>
public sealed record UpdatePerformanceReviewResponse(
    DefaultIdType Id,
    decimal OverallRating,
    string Status);

