namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeContacts.Create.v1;

/// <summary>
/// Handler for creating an employee contact.
/// </summary>
public sealed class CreateEmployeeContactHandler(
    ILogger<CreateEmployeeContactHandler> logger,
    [FromKeyedServices("hr:employees")] IReadRepository<Employee> employeeRepository,
    [FromKeyedServices("hr:contacts")] IRepository<EmployeeContact> repository)
    : IRequestHandler<CreateEmployeeContactCommand, CreateEmployeeContactResponse>
{
    public async Task<CreateEmployeeContactResponse> Handle(
        CreateEmployeeContactCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Verify employee exists
        var employee = await employeeRepository
            .GetByIdAsync(request.EmployeeId, cancellationToken)
            .ConfigureAwait(false);

        if (employee is null)
            throw new EmployeeNotFoundException(request.EmployeeId);

        // Create contact
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
            "Employee contact created with ID {ContactId}, Employee {EmployeeId}, Type {ContactType}",
            contact.Id,
            contact.EmployeeId,
            contact.ContactType);

        return new CreateEmployeeContactResponse(contact.Id);
    }
}

