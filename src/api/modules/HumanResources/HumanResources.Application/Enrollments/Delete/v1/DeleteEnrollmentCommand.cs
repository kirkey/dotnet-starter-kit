namespace FSH.Starter.WebApi.HumanResources.Application.Enrollments.Delete.v1;

/// <summary>
/// Command to delete an enrollment.
/// </summary>
public sealed record DeleteEnrollmentCommand(DefaultIdType Id) : IRequest<DeleteEnrollmentResponse>;

