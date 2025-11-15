namespace FSH.Starter.WebApi.HumanResources.Application.Employees.Terminate.v1;

/// <summary>
/// Handler for terminating an employee per Philippines Labor Code.
/// Computes separation pay based on years of service and separation pay basis.
/// </summary>
public sealed class TerminateEmployeeHandler(
    ILogger<TerminateEmployeeHandler> logger,
    [FromKeyedServices("hr:employees")] IRepository<Employee> repository)
    : IRequestHandler<TerminateEmployeeCommand, TerminateEmployeeResponse>
{
    public async Task<TerminateEmployeeResponse> Handle(TerminateEmployeeCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var employee = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (employee is null)
        {
            throw new EmployeeNotFoundException(request.Id);
        }

        // Verify employee is not already terminated
        if (employee.Status == "Terminated")
        {
            throw new InvalidEmployeeTerminationException(request.Id, employee.Status);
        }

        // Terminate employee using domain method
        employee.Terminate(
            request.TerminationDate,
            request.TerminationReason,
            request.TerminationMode,
            request.SeparationPayBasis,
            request.SeparationPayAmount);

        // Calculate separation pay
        var separationPay = employee.CalculateSeparationPay();

        // Persist to database
        await repository.UpdateAsync(employee, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Employee {EmployeeId} ({EmployeeNumber}) terminated on {TerminationDate}. Reason: {Reason}, Separation Pay: ₱{SeparationPay:N2}",
            employee.Id,
            employee.EmployeeNumber,
            request.TerminationDate,
            request.TerminationReason,
            separationPay);

        return new TerminateEmployeeResponse(employee.Id, request.TerminationDate, separationPay);
    }
}

