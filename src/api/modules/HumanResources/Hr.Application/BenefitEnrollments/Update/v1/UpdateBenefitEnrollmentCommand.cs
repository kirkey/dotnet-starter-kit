namespace FSH.Starter.WebApi.HumanResources.Application.BenefitEnrollments.Update.v1;

/// <summary>
/// Command to update benefit enrollment.
/// </summary>
public sealed record UpdateBenefitEnrollmentCommand(
    DefaultIdType Id,
    [property: DefaultValue(null)] string? CoverageLevel = null,
    [property: DefaultValue(null)] decimal? EmployeeContributionAmount = null,
    [property: DefaultValue(null)] decimal? EmployerContributionAmount = null,
    [property: DefaultValue(null)] DefaultIdType[]? AddDependentIds = null
) : IRequest<UpdateBenefitEnrollmentResponse>;

/// <summary>
/// Response for benefit enrollment update.
/// </summary>
public sealed record UpdateBenefitEnrollmentResponse(
    DefaultIdType Id,
    string? CoverageLevel,
    decimal AnnualContribution,
    bool IsActive);

