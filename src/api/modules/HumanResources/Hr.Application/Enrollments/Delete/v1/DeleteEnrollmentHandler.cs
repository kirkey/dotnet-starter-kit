namespace FSH.Starter.WebApi.HumanResources.Application.Enrollments.Delete.v1;

/// <summary>
/// Handler for deleting an enrollment.
/// </summary>
public sealed class DeleteEnrollmentHandler(
    ILogger<DeleteEnrollmentHandler> logger,
    [FromKeyedServices("hr:enrollments")] IRepository<BenefitEnrollment> repository)
    : IRequestHandler<DeleteEnrollmentCommand, DeleteEnrollmentResponse>
{
    public async Task<DeleteEnrollmentResponse> Handle(
        DeleteEnrollmentCommand request,
        CancellationToken cancellationToken)
    {
        var enrollment = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (enrollment is null)
            throw new Exception($"Enrollment not found: {request.Id}");

        await repository.DeleteAsync(enrollment, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Enrollment {EnrollmentId} deleted successfully", enrollment.Id);

        return new DeleteEnrollmentResponse(enrollment.Id);
    }
}

