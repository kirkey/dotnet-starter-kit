namespace FSH.Starter.WebApi.HumanResources.Application.Enrollments.Create.v1;

/// <summary>
/// Handler for creating an enrollment.
/// </summary>
public sealed class CreateEnrollmentHandler(
    ILogger<CreateEnrollmentHandler> logger,
    [FromKeyedServices("hr:enrollments")] IRepository<BenefitEnrollment> repository,
    [FromKeyedServices("hr:employees")] IReadRepository<Employee> employeeRepository,
    [FromKeyedServices("hr:benefits")] IReadRepository<Benefit> benefitRepository)
    : IRequestHandler<CreateEnrollmentCommand, CreateEnrollmentResponse>
{
    public async Task<CreateEnrollmentResponse> Handle(
        CreateEnrollmentCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var employee = await employeeRepository
            .GetByIdAsync(request.EmployeeId, cancellationToken)
            .ConfigureAwait(false);

        if (employee is null)
            throw new Exception($"Employee not found: {request.EmployeeId}");

        var benefit = await benefitRepository
            .GetByIdAsync(request.BenefitId, cancellationToken)
            .ConfigureAwait(false);

        if (benefit is null)
            throw new Exception($"Benefit not found: {request.BenefitId}");

        var enrollment = BenefitEnrollment.Create(
            request.EmployeeId,
            request.BenefitId,
            request.EnrollmentDate,
            request.EffectiveDate);

        if (!string.IsNullOrWhiteSpace(request.CoverageLevel) && request.EmployeeContributionAmount.HasValue && request.EmployerContributionAmount.HasValue)
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

        await repository.AddAsync(enrollment, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Enrollment created with ID {EnrollmentId}, Employee {EmployeeId}, Benefit {BenefitId}",
            enrollment.Id,
            request.EmployeeId,
            request.BenefitId);

        return new CreateEnrollmentResponse(enrollment.Id);
    }
}

