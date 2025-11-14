namespace FSH.Starter.WebApi.HumanResources.Application.BenefitEnrollments.Get.v1;

/// <summary>
/// Request to get benefit enrollment details.
/// </summary>
public sealed record GetBenefitEnrollmentRequest(DefaultIdType Id) : IRequest<BenefitEnrollmentResponse>;

/// <summary>
/// Response with benefit enrollment details.
/// </summary>
public sealed record BenefitEnrollmentResponse(
    DefaultIdType Id,
    DefaultIdType EmployeeId,
    string EmployeeName,
    DefaultIdType BenefitId,
    string BenefitName,
    DateTime EnrollmentDate,
    DateTime EffectiveDate,
    DateTime? EndDate,
    string? CoverageLevel,
    decimal EmployeeContributionAmount,
    decimal EmployerContributionAmount,
    decimal AnnualContribution,
    bool IsActive,
    string? CoveredDependentIds);

