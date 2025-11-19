namespace FSH.Starter.WebApi.HumanResources.Application.Enrollments.Terminate.v1;

/// <summary>
/// Handler for terminating an enrollment.
/// </summary>
public sealed class TerminateEnrollmentHandler(
    ILogger<TerminateEnrollmentHandler> logger,
    [FromKeyedServices("hr:enrollments")] IRepository<BenefitEnrollment> repository)
    : IRequestHandler<TerminateEnrollmentCommand, TerminateEnrollmentResponse>
{
    public async Task<TerminateEnrollmentResponse> Handle(
        TerminateEnrollmentCommand request,
        CancellationToken cancellationToken)
    {
        var enrollment = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (enrollment is null)
            throw new Exception($"Enrollment not found: {request.Id}");

        enrollment.Terminate(request.EndDate);

        await repository.UpdateAsync(enrollment, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Enrollment {EnrollmentId} terminated successfully", enrollment.Id);

        return new TerminateEnrollmentResponse(enrollment.Id);
    }
}

