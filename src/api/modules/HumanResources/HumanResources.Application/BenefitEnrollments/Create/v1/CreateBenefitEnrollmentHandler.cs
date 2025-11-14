namespace FSH.Starter.WebApi.HumanResources.Application.BenefitEnrollments.Create.v1;

/// <summary>
/// Handler for creating benefit enrollment.
/// </summary>
public sealed class CreateBenefitEnrollmentHandler(
    ILogger<CreateBenefitEnrollmentHandler> logger,
    [FromKeyedServices("hr:benefitenrollments")] IRepository<BenefitEnrollment> repository,
    [FromKeyedServices("hr:employees")] IReadRepository<Employee> employeeRepository,
    [FromKeyedServices("hr:benefits")] IReadRepository<Benefit> benefitRepository)
    : IRequestHandler<CreateBenefitEnrollmentCommand, CreateBenefitEnrollmentResponse>
{
    public async Task<CreateBenefitEnrollmentResponse> Handle(
        CreateBenefitEnrollmentCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Verify employee exists
        var employee = await employeeRepository.GetByIdAsync(request.EmployeeId, cancellationToken);
        if (employee is null)
            throw new EmployeeNotFoundException(request.EmployeeId);

        // Verify benefit exists
        var benefit = await benefitRepository.GetByIdAsync(request.BenefitId, cancellationToken);
        if (benefit is null)
            throw new BenefitNotFoundException(request.BenefitId);

        // Use provided dates or defaults
        var enrollmentDate = request.EnrollmentDate ?? DateTime.UtcNow;
        var effectiveDate = request.EffectiveDate ?? DateTime.UtcNow.AddDays(benefit.WaitingPeriodDays ?? 0);

        // Create enrollment
        var enrollment = BenefitEnrollment.Create(
            request.EmployeeId,
            request.BenefitId,
            enrollmentDate,
            effectiveDate);

        // Set coverage details
        enrollment.SetCoverage(
            request.CoverageLevel,
            request.EmployeeContributionAmount,
            request.EmployerContributionAmount);

        // Add dependents if provided
        if (request.DependentIds != null && request.DependentIds.Length > 0)
        {
            enrollment.AddDependents(request.DependentIds);
        }

        await repository.AddAsync(enrollment, cancellationToken);

        logger.LogInformation(
            "Benefit enrollment created: Employee {EmployeeId}, Benefit {BenefitId}, Coverage {Coverage}",
            enrollment.EmployeeId,
            enrollment.BenefitId,
            enrollment.CoverageLevel);

        return new CreateBenefitEnrollmentResponse(
            enrollment.Id,
            enrollment.EmployeeId,
            enrollment.BenefitId,
            enrollment.EnrollmentDate,
            enrollment.EffectiveDate,
            enrollment.CoverageLevel,
            enrollment.AnnualContribution,
            enrollment.IsActive);
    }
}

