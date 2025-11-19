namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeContacts.Create.v1;

/// <summary>
/// Handler for creating a new employee contact.
/// </summary>
public sealed class CreateEmployeeContactHandler(
    ILogger<CreateEmployeeContactHandler> logger,
    [FromKeyedServices("hr:employeecontacts")] IRepository<EmployeeContact> repository,
    [FromKeyedServices("hr:employees")] IReadRepository<Employee> employeeRepository)
    : IRequestHandler<CreateEmployeeContactCommand, CreateEmployeeContactResponse>
{
    /// <summary>
    /// Handles the request to create an employee contact.
    /// </summary>
    public async Task<CreateEmployeeContactResponse> Handle(
        CreateEmployeeContactCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var employee = await employeeRepository
            .GetByIdAsync(request.EmployeeId, cancellationToken)
            .ConfigureAwait(false);

        if (employee is null)
            throw new EmployeeNotFoundException(request.EmployeeId);

        var contact = EmployeeContact.Create(
            request.EmployeeId,
            request.FirstName,
            request.LastName,
            request.ContactType,
            request.Relationship,
            request.PhoneNumber,
            request.Email,
            request.Address);

        await repository.AddAsync(contact, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Employee contact created with ID {ContactId}, Name {FullName} for Employee {EmployeeId}",
            contact.Id,
            contact.FullName,
            employee.Id);

        return new CreateEmployeeContactResponse(contact.Id);
    }
}

