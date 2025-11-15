namespace FSH.Starter.WebApi.HumanResources.Application.Enrollments.Update.v1;

/// <summary>
/// Handler for updating an enrollment.
/// </summary>
public sealed class UpdateEnrollmentHandler(
    ILogger<UpdateEnrollmentHandler> logger,
    [FromKeyedServices("hr:enrollments")] IRepository<BenefitEnrollment> repository)
    : IRequestHandler<UpdateEnrollmentCommand, UpdateEnrollmentResponse>
{
    public async Task<UpdateEnrollmentResponse> Handle(
        UpdateEnrollmentCommand request,
        CancellationToken cancellationToken)
    {
        var enrollment = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (enrollment is null)
            throw new Exception($"Enrollment not found: {request.Id}");

        if (!string.IsNullOrWhiteSpace(request.CoverageLevel) && request is { EmployeeContributionAmount: not null, EmployerContributionAmount: not null })
        {
            enrollment.SetCoverage(
                request.CoverageLevel,
                request.EmployeeContributionAmount.Value,
                request.EmployerContributionAmount.Value);
        }

        if (request.CoveredDependentIds?.Length > 0)
        {
            enrollment.AddDependents(request.CoveredDependentIds);
        }

        await repository.UpdateAsync(enrollment, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Enrollment {EnrollmentId} updated successfully", enrollment.Id);

        return new UpdateEnrollmentResponse(enrollment.Id);
    }
}

