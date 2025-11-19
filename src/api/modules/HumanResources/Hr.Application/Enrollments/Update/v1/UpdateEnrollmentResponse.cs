namespace FSH.Starter.WebApi.HumanResources.Application.Enrollments.Update.v1;

/// <summary>
/// Response for updating an enrollment.
/// </summary>
/// <param name="Id">The identifier of the updated enrollment.</param>
public sealed record UpdateEnrollmentResponse(DefaultIdType Id);

