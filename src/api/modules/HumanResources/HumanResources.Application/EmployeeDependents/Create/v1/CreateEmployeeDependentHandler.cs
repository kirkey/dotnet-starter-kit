namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Create.v1;

/// <summary>
/// Handler for creating an employee dependent.
/// </summary>
public sealed class CreateEmployeeDependentHandler(
    ILogger<CreateEmployeeDependentHandler> logger,
    [FromKeyedServices("hr:employees")] IReadRepository<Employee> employeeRepository,
    [FromKeyedServices("hr:dependents")] IRepository<EmployeeDependent> repository)
    : IRequestHandler<CreateEmployeeDependentCommand, CreateEmployeeDependentResponse>
{
    public async Task<CreateEmployeeDependentResponse> Handle(
        CreateEmployeeDependentCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Verify employee exists
        var employee = await employeeRepository
            .GetByIdAsync(request.EmployeeId, cancellationToken)
            .ConfigureAwait(false);

        if (employee is null)
            throw new EmployeeNotFoundException(request.EmployeeId);

        // Create dependent
        var dependent = EmployeeDependent.Create(
            request.EmployeeId,
            request.FirstName,
            request.LastName,
            request.DependentType,
            request.DateOfBirth,
            request.Relationship,
            request.Ssn,
            request.Email,
            request.PhoneNumber);

        await repository.AddAsync(dependent, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Employee dependent created with ID {DependentId}, Employee {EmployeeId}, Type {DependentType}",
            dependent.Id,
            dependent.EmployeeId,
            dependent.DependentType);

        return new CreateEmployeeDependentResponse(dependent.Id);
    }
}

