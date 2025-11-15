namespace FSH.Starter.WebApi.HumanResources.Application.BenefitEnrollments.Update.v1;

/// <summary>
/// Handler for updating benefit enrollment.
/// </summary>
public sealed class UpdateBenefitEnrollmentHandler(
    ILogger<UpdateBenefitEnrollmentHandler> logger,
    [FromKeyedServices("hr:benefitenrollments")] IRepository<BenefitEnrollment> repository)
    : IRequestHandler<UpdateBenefitEnrollmentCommand, UpdateBenefitEnrollmentResponse>
{
    public async Task<UpdateBenefitEnrollmentResponse> Handle(
        UpdateBenefitEnrollmentCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var enrollment = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (enrollment is null)
            throw new BenefitEnrollmentNotFoundException(request.Id);

        // Update coverage if provided
        if (!string.IsNullOrWhiteSpace(request.CoverageLevel) ||
            request.EmployeeContributionAmount.HasValue ||
            request.EmployerContributionAmount.HasValue)
        {
            enrollment.SetCoverage(
                request.CoverageLevel ?? enrollment.CoverageLevel ?? "Individual",
                request.EmployeeContributionAmount ?? enrollment.EmployeeContributionAmount,
                request.EmployerContributionAmount ?? enrollment.EmployerContributionAmount);
        }

        // Add dependents if provided
        if (request.AddDependentIds is { Length: > 0 })
        {
            enrollment.AddDependents(request.AddDependentIds);
        }

        await repository.UpdateAsync(enrollment, cancellationToken);

        logger.LogInformation("Benefit enrollment {Id} updated", enrollment.Id);

        return new UpdateBenefitEnrollmentResponse(
            enrollment.Id,
            enrollment.CoverageLevel,
            enrollment.AnnualContribution,
            enrollment.IsActive);
    }
}

