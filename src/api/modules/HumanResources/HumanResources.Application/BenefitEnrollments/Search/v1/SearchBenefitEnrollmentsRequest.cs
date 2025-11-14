namespace FSH.Starter.WebApi.HumanResources.Application.BenefitEnrollments.Search.v1;

/// <summary>
/// Request to search benefit enrollments.
/// </summary>
public sealed record SearchBenefitEnrollmentsRequest(
    [property: DefaultValue(null)] DefaultIdType? EmployeeId = null,
    [property: DefaultValue(null)] DefaultIdType? BenefitId = null,
    [property: DefaultValue(null)] bool? IsActive = null,
    [property: DefaultValue(null)] string? CoverageLevel = null,
    [property: DefaultValue(1)] int PageNumber = 1,
    [property: DefaultValue(10)] int PageSize = 10
) : IRequest<PagedList<BenefitEnrollmentDto>>;

/// <summary>
/// DTO for benefit enrollment search results.
/// </summary>
public sealed record BenefitEnrollmentDto(
    DefaultIdType Id,
    DefaultIdType EmployeeId,
    string EmployeeName,
    DefaultIdType BenefitId,
    string BenefitName,
    DateTime EffectiveDate,
    string? CoverageLevel,
    decimal AnnualContribution,
    bool IsActive);

