namespace FSH.Starter.WebApi.HumanResources.Application.Enrollments.Terminate.v1;

/// <summary>
/// Response for terminating an enrollment.
/// </summary>
/// <param name="Id">The identifier of the terminated enrollment.</param>
public sealed record TerminateEnrollmentResponse(DefaultIdType Id);

