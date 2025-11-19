namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Create.v1;

/// <summary>
/// Handler for creating a new employee dependent.
/// </summary>
public sealed class CreateEmployeeDependentHandler(
    ILogger<CreateEmployeeDependentHandler> logger,
    [FromKeyedServices("hr:dependents")] IRepository<EmployeeDependent> repository,
    [FromKeyedServices("hr:employees")] IReadRepository<Employee> employeeRepository)
    : IRequestHandler<CreateEmployeeDependentCommand, CreateEmployeeDependentResponse>
{
    /// <summary>
    /// Handles the request to create an employee dependent.
    /// </summary>
    public async Task<CreateEmployeeDependentResponse> Handle(
        CreateEmployeeDependentCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var employee = await employeeRepository
            .GetByIdAsync(request.EmployeeId, cancellationToken)
            .ConfigureAwait(false);

        if (employee is null)
            throw new EmployeeNotFoundException(request.EmployeeId);

        var dependent = EmployeeDependent.Create(
            request.EmployeeId,
            request.FirstName,
            request.LastName,
            request.DependentType,
            request.DateOfBirth,
            request.Relationship,
            null,
            request.Email,
            request.PhoneNumber);

        await repository.AddAsync(dependent, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Employee dependent created with ID {DependentId}, Name {FullName} for Employee {EmployeeId}",
            dependent.Id,
            dependent.FullName,
            employee.Id);

        return new CreateEmployeeDependentResponse(dependent.Id);
    }
}
