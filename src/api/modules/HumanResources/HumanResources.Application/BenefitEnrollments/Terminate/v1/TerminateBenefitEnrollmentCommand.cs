namespace FSH.Starter.WebApi.HumanResources.Application.BenefitEnrollments.Terminate.v1;

/// <summary>
/// Command to terminate benefit enrollment.
/// </summary>
public sealed record TerminateBenefitEnrollmentCommand(
    DefaultIdType Id,
    [property: DefaultValue(null)] DateTime? EndDate = null
) : IRequest<TerminateBenefitEnrollmentResponse>;

/// <summary>
/// Response for benefit enrollment termination.
/// </summary>
public sealed record TerminateBenefitEnrollmentResponse(
    DefaultIdType Id,
    DateTime EndDate,
    bool IsActive);

