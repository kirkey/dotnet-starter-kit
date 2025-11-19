namespace FSH.Starter.WebApi.HumanResources.Application.Enrollments.Update.v1;

/// <summary>
/// Command to update an enrollment.
/// </summary>
public sealed record UpdateEnrollmentCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType Id,
    [property: DefaultValue(null)] string? CoverageLevel = null,
    [property: DefaultValue(null)] decimal? EmployeeContributionAmount = null,
    [property: DefaultValue(null)] decimal? EmployerContributionAmount = null,
    [property: DefaultValue(null)] DefaultIdType[]? CoveredDependentIds = null) : IRequest<UpdateEnrollmentResponse>;

