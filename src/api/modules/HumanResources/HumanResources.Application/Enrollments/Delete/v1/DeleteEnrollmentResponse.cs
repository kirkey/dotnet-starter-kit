namespace FSH.Starter.WebApi.HumanResources.Application.Enrollments.Delete.v1;

/// <summary>
/// Response for deleting an enrollment.
/// </summary>
/// <param name="Id">The identifier of the deleted enrollment.</param>
public sealed record DeleteEnrollmentResponse(DefaultIdType Id);

