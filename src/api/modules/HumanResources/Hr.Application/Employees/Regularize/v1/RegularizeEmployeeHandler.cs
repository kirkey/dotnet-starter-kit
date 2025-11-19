namespace FSH.Starter.WebApi.HumanResources.Application.Employees.Regularize.v1;

/// <summary>
/// Handler for regularizing a probationary employee per Philippines Labor Code Article 280.
/// Converts Probationary status to Regular employment.
/// </summary>
public sealed class RegularizeEmployeeHandler(
    ILogger<RegularizeEmployeeHandler> logger,
    [FromKeyedServices("hr:employees")] IRepository<Employee> repository)
    : IRequestHandler<RegularizeEmployeeCommand, RegularizeEmployeeResponse>
{
    public async Task<RegularizeEmployeeResponse> Handle(RegularizeEmployeeCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var employee = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (employee is null)
        {
            throw new EmployeeNotFoundException(request.Id);
        }

        // Regularize employee using domain method
        employee.Regularize(request.RegularizationDate);

        // Persist to database
        await repository.UpdateAsync(employee, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Employee {EmployeeId} ({EmployeeNumber}) regularized on {RegularizationDate}. Classification changed to Regular.",
            employee.Id,
            employee.EmployeeNumber,
            request.RegularizationDate);

        return new RegularizeEmployeeResponse(employee.Id, request.RegularizationDate);
    }
}

