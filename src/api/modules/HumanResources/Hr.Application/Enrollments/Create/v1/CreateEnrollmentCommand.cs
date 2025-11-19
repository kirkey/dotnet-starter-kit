namespace FSH.Starter.WebApi.HumanResources.Application.Enrollments.Create.v1;

/// <summary>
/// Command to create a new enrollment.
/// </summary>
public sealed record CreateEnrollmentCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType EmployeeId,
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType BenefitId,
    [property: DefaultValue("2025-11-14")] DateTime EnrollmentDate,
    [property: DefaultValue("2025-12-01")] DateTime EffectiveDate,
    [property: DefaultValue(null)] string? CoverageLevel = null,
    [property: DefaultValue(null)] decimal? EmployeeContributionAmount = null,
    [property: DefaultValue(null)] decimal? EmployerContributionAmount = null,
    [property: DefaultValue(null)] DefaultIdType[]? CoveredDependentIds = null) : IRequest<CreateEnrollmentResponse>;

