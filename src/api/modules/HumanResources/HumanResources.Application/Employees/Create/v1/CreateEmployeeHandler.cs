using FSH.Starter.WebApi.HumanResources.Application.Employees.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.Employees.Create.v1;

/// <summary>
/// Handler for creating a new employee.
/// </summary>
public sealed class CreateEmployeeHandler(
    ILogger<CreateEmployeeHandler> logger,
    [FromKeyedServices("hr:employees")] IRepository<Employee> repository,
    [FromKeyedServices("hr:employees")] IReadRepository<Employee> readRepository)
    : IRequestHandler<CreateEmployeeCommand, CreateEmployeeResponse>
{
    public async Task<CreateEmployeeResponse> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Check if employee number already exists
        var existingEmployee = await readRepository
            .FirstOrDefaultAsync(
                new EmployeeByNumberSpec(request.EmployeeNumber),
                cancellationToken)
            .ConfigureAwait(false);

        if (existingEmployee is not null)
        {
            throw new EmployeeNumberAlreadyExistsException(request.EmployeeNumber);
        }

        // Create employee using domain factory method
        var employee = Employee.Create(
            request.EmployeeNumber,
            request.FirstName,
            request.LastName,
            request.OrganizationalUnitId,
            request.MiddleName,
            request.Email,
            request.PhoneNumber);

        // Persist to database
        await repository.AddAsync(employee, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Employee created with ID {EmployeeId}, Number {EmployeeNumber}, Name {EmployeeName}",
            employee.Id,
            employee.EmployeeNumber,
            employee.FullName);

        return new CreateEmployeeResponse(employee.Id);
    }
}

