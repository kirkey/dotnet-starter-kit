namespace FSH.Starter.WebApi.HumanResources.Application.Employees.Regularize.v1;

/// <summary>
/// Handler for regularizing probationary employee per Philippines Labor Code.
/// Converts employment classification from Probationary to Regular.
/// </summary>
public sealed class RegularizeEmployeeHandler(
    ILogger<RegularizeEmployeeHandler> logger,
    [FromKeyedServices("hr:employees")] IRepository<Employee> repository)
    : IRequestHandler<RegularizeEmployeeCommand, RegularizeEmployeeResponse>
{
    public async Task<RegularizeEmployeeResponse> Handle(
        RegularizeEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        var employee = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (employee is null)
            throw new EmployeeNotFoundException(request.Id);

        if (employee.EmploymentClassification != "Probationary")
        {
            throw new InvalidOperationException(
                $"Cannot regularize employee with classification '{employee.EmploymentClassification}'. Only Probationary employees can be regularized.");
        }

        if (!employee.HireDate.HasValue)
        {
            throw new InvalidOperationException("Employee must have a hire date to be regularized.");
        }

        // Validate regularization date is after hire date
        if (request.RegularizationDate < employee.HireDate.Value)
        {
            throw new InvalidOperationException("Regularization date must be on or after hire date.");
        }

        // Philippines Labor Code: Probation period typically 6 months for general, 12 months for technical
        var monthsSinceHire = (request.RegularizationDate - employee.HireDate.Value).TotalDays / 30.44;
        if (monthsSinceHire < 6)
        {
            logger.LogWarning(
                "Employee {EmployeeId} is being regularized after only {Months:F1} months. Typical probation is 6 months.",
                employee.Id,
                monthsSinceHire);
        }

        employee.Regularize(request.RegularizationDate);

        await repository.UpdateAsync(employee, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Employee {EmployeeId} regularized on {RegularizationDate}. Probation duration: {Months:F1} months.",
            employee.Id,
            request.RegularizationDate,
            monthsSinceHire);

        return new RegularizeEmployeeResponse(employee.Id, request.RegularizationDate);
    }
}

