namespace FSH.Starter.WebApi.HumanResources.Application.Employees.Create.v1;

public sealed class CreateEmployeeHandler(
    ILogger<CreateEmployeeHandler> logger,
    [FromKeyedServices("hr:employees")] IRepository<Employee> repository,
    [FromKeyedServices("hr:organizationalunits")] IReadRepository<OrganizationalUnit> organizationalUnitRepository)
    : IRequestHandler<CreateEmployeeCommand, CreateEmployeeResponse>
{
    public async Task<CreateEmployeeResponse> Handle(
        CreateEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var organizationalUnit = await organizationalUnitRepository
            .GetByIdAsync(request.OrganizationalUnitId, cancellationToken)
            .ConfigureAwait(false);

        if (organizationalUnit is null)
            throw new OrganizationalUnitNotFoundException(request.OrganizationalUnitId);

        var employee = Employee.Create(
            request.EmployeeNumber,
            request.FirstName,
            request.LastName,
            request.OrganizationalUnitId,
            request.MiddleName,
            request.Email,
            request.PhoneNumber);

        if (request.HireDate.HasValue)
            employee.SetHireDate(request.HireDate.Value);

        await repository.AddAsync(employee, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Employee created with ID {EmployeeId}, Number {EmployeeNumber}, Name {FullName}",
            employee.Id,
            employee.EmployeeNumber,
            employee.FullName);

        return new CreateEmployeeResponse(employee.Id);
    }
}

