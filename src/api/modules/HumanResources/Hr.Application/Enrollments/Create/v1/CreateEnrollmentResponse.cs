namespace FSH.Starter.WebApi.HumanResources.Application.Enrollments.Create.v1;

/// <summary>
/// Response for creating an enrollment.
/// </summary>
/// <param name="Id">The identifier of the created enrollment.</param>
public sealed record CreateEnrollmentResponse(DefaultIdType Id);

