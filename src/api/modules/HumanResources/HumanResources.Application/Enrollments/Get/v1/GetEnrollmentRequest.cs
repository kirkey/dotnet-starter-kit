namespace FSH.Starter.WebApi.HumanResources.Application.Enrollments.Get.v1;

/// <summary>
/// Request to get an enrollment by its identifier.
/// </summary>
public sealed record GetEnrollmentRequest(DefaultIdType Id) : IRequest<EnrollmentResponse>;

