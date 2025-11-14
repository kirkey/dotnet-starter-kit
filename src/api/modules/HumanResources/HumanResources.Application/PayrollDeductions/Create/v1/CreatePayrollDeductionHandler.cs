namespace FSH.Starter.WebApi.HumanResources.Application.PayrollDeductions.Create.v1;

/// <summary>
/// Handler for creating payroll deduction with Philippines Labor Code compliance.
/// Validates deduction authorization and applicability rules.
/// </summary>
public sealed class CreatePayrollDeductionHandler(
    ILogger<CreatePayrollDeductionHandler> logger,
    [FromKeyedServices("hr:payrolldeductions")] IRepository<PayrollDeduction> repository,
    [FromKeyedServices("hr:paycomponents")] IReadRepository<PayComponent> componentRepository,
    [FromKeyedServices("hr:employees")] IReadRepository<Employee> employeeRepository,
    [FromKeyedServices("hr:organizationalunits")] IReadRepository<OrganizationalUnit> unitRepository)
    : IRequestHandler<CreatePayrollDeductionCommand, CreatePayrollDeductionResponse>
{
    public async Task<CreatePayrollDeductionResponse> Handle(
        CreatePayrollDeductionCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Validate PayComponent exists
        var component = await componentRepository.GetByIdAsync(request.PayComponentId, cancellationToken);
        if (component is null)
            throw new PayComponentNotFoundException(request.PayComponentId);

        // Validate employee or organizational unit (one must be specified)
        if (request.EmployeeId.HasValue)
        {
            var employee = await employeeRepository.GetByIdAsync(request.EmployeeId.Value, cancellationToken);
            if (employee is null)
                throw new EmployeeNotFoundException(request.EmployeeId.Value);
        }
        else if (request.OrganizationalUnitId.HasValue)
        {
            var unit = await unitRepository.GetByIdAsync(request.OrganizationalUnitId.Value, cancellationToken);
            if (unit is null)
                throw new OrganizationalUnitNotFoundException(request.OrganizationalUnitId.Value);
        }
        else
        {
            throw new InvalidOperationException("Either EmployeeId or OrganizationalUnitId must be specified.");
        }

        // Create deduction
        var deduction = PayrollDeduction.Create(
            request.PayComponentId,
            request.DeductionType,
            request.DeductionAmount,
            request.DeductionPercentage);

        // Set scope (employee or area)
        if (request.EmployeeId.HasValue)
            deduction.SetEmployee(request.EmployeeId.Value);
        else
            deduction.SetOrganizationalUnit(request.OrganizationalUnitId!.Value);

        // Set date range
        deduction.SetDateRange(request.StartDate, request.EndDate);

        // Set max deduction limit (Philippines Labor Code Art 113: 70% of wages)
        if (request.MaxDeductionLimit.HasValue)
            deduction.SetMaxDeductionLimit(request.MaxDeductionLimit.Value);
        else
            deduction.SetMaxDeductionLimit(0); // No limit if not specified

        // Set reference and remarks
        if (!string.IsNullOrWhiteSpace(request.ReferenceNumber))
            deduction.SetReferenceNumber(request.ReferenceNumber);

        if (!string.IsNullOrWhiteSpace(request.Remarks))
            deduction.UpdateRemarks(request.Remarks);

        // Authorize deduction (HR to confirm it's authorized per Labor Code)
        deduction.SetAsAuthorized(IsAuthorizedDeductionType(component.ComponentName));

        // Set recovery policy (default: recoverable unless prohibited)
        deduction.SetRecoverable(!IsNonRecoverableDeduction(component.ComponentName));

        await repository.AddAsync(deduction, cancellationToken);

        logger.LogInformation(
            "Payroll deduction created: ID {DeductionId}, Type {Type}, Amount {Amount}, " +
            "Employee {EmployeeId}, Area {AreaId}",
            deduction.Id,
            request.DeductionType,
            request.DeductionAmount > 0 ? request.DeductionAmount : request.DeductionPercentage + "%",
            request.EmployeeId,
            request.OrganizationalUnitId);

        return new CreatePayrollDeductionResponse(
            deduction.Id,
            deduction.DeductionType,
            request.DeductionAmount > 0 ? request.DeductionAmount : 0,
            deduction.IsAuthorized);
    }

    /// <summary>
    /// Checks if component represents authorized deduction per Labor Code.
    /// </summary>
    private static bool IsAuthorizedDeductionType(string componentName)
    {
        var authorized = new[]
        {
            "Loan", "Insurance", "Union Dues", "Court Order", "Garnishment",
            "Cooperative", "Savings", "Health", "Retirement", "SSS", "PhilHealth", "Pag-IBIG"
        };

        return authorized.Any(a => componentName.Contains(a, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Checks if deduction cannot be recovered per Labor Code.
    /// </summary>
    private static bool IsNonRecoverableDeduction(string componentName)
    {
        var nonRecoverable = new[]
        {
            "Income Tax", "SSS", "PhilHealth", "Pag-IBIG", "Court Order", "Garnishment"
        };

        return nonRecoverable.Any(n => componentName.Contains(n, StringComparison.OrdinalIgnoreCase));
    }
}

