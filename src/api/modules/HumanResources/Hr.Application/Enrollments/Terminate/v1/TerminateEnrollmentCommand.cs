namespace FSH.Starter.WebApi.HumanResources.Application.Enrollments.Terminate.v1;

/// <summary>
/// Command to terminate an enrollment.
/// </summary>
public sealed record TerminateEnrollmentCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType Id,
    [property: DefaultValue("2025-12-31")] DateTime EndDate) : IRequest<TerminateEnrollmentResponse>;

