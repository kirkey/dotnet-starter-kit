namespace FSH.Starter.WebApi.HumanResources.Application.PerformanceReviews.Search.v1;

/// <summary>
/// Request to search performance reviews.
/// </summary>
public sealed record SearchPerformanceReviewsRequest(
    [property: DefaultValue(null)] DefaultIdType? EmployeeId = null,
    [property: DefaultValue(null)] DefaultIdType? ReviewerId = null,
    [property: DefaultValue(null)] string? Status = null,
    [property: DefaultValue(null)] string? ReviewType = null,
    [property: DefaultValue(null)] DateTime? PeriodStart = null,
    [property: DefaultValue(null)] DateTime? PeriodEnd = null,
    [property: DefaultValue(1)] int PageNumber = 1,
    [property: DefaultValue(10)] int PageSize = 10
) : IRequest<PagedList<PerformanceReviewDto>>;

/// <summary>
/// DTO for performance review search results.
/// </summary>
public sealed record PerformanceReviewDto(
    DefaultIdType Id,
    DefaultIdType EmployeeId,
    string EmployeeName,
    DefaultIdType ReviewerId,
    string ReviewerName,
    DateTime ReviewPeriodEnd,
    string ReviewType,
    string Status,
    decimal OverallRating);

