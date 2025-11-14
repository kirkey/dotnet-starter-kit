namespace FSH.Starter.WebApi.HumanResources.Application.BenefitEnrollments.Create.v1;

/// <summary>
/// Command to enroll employee in a benefit.
/// </summary>
public sealed record CreateBenefitEnrollmentCommand(
    DefaultIdType EmployeeId,
    DefaultIdType BenefitId,
    [property: DefaultValue(null)] DateTime? EnrollmentDate = null,
    [property: DefaultValue(null)] DateTime? EffectiveDate = null,
    [property: DefaultValue("Individual")] string CoverageLevel = "Individual",
    [property: DefaultValue(0)] decimal EmployeeContributionAmount = 0,
    [property: DefaultValue(0)] decimal EmployerContributionAmount = 0,
    [property: DefaultValue(null)] DefaultIdType[]? DependentIds = null
) : IRequest<CreateBenefitEnrollmentResponse>;

/// <summary>
/// Response for benefit enrollment creation.
/// </summary>
public sealed record CreateBenefitEnrollmentResponse(
    DefaultIdType Id,
    DefaultIdType EmployeeId,
    DefaultIdType BenefitId,
    DateTime EnrollmentDate,
    DateTime EffectiveDate,
    string? CoverageLevel,
    decimal AnnualContribution,
    bool IsActive);

