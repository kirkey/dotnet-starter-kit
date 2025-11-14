namespace FSH.Starter.WebApi.HumanResources.Application.BenefitEnrollments.Terminate.v1;

/// <summary>
/// Handler for terminating benefit enrollment.
/// </summary>
public sealed class TerminateBenefitEnrollmentHandler(
    ILogger<TerminateBenefitEnrollmentHandler> logger,
    [FromKeyedServices("hr:benefitenrollments")] IRepository<BenefitEnrollment> repository)
    : IRequestHandler<TerminateBenefitEnrollmentCommand, TerminateBenefitEnrollmentResponse>
{
    public async Task<TerminateBenefitEnrollmentResponse> Handle(
        TerminateBenefitEnrollmentCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var enrollment = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (enrollment is null)
            throw new BenefitEnrollmentNotFoundException(request.Id);

        var endDate = request.EndDate ?? DateTime.UtcNow;
        enrollment.Terminate(endDate);

        await repository.UpdateAsync(enrollment, cancellationToken);

        logger.LogInformation("Benefit enrollment {Id} terminated on {EndDate}", enrollment.Id, endDate);

        return new TerminateBenefitEnrollmentResponse(
            enrollment.Id,
            enrollment.EndDate!.Value,
            enrollment.IsActive);
    }
}

