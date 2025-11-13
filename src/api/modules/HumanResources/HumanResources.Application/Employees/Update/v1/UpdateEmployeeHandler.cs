namespace FSH.Starter.WebApi.HumanResources.Application.Employees.Update.v1;

/// <summary>
/// Handler for updating employee.
/// </summary>
public sealed class UpdateEmployeeHandler(
    ILogger<UpdateEmployeeHandler> logger,
    [FromKeyedServices("hr:employees")] IRepository<Employee> repository)
    : IRequestHandler<UpdateEmployeeCommand, UpdateEmployeeResponse>
{
    public async Task<UpdateEmployeeResponse> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (employee is null)
        {
            throw new EmployeeNotFoundException(request.Id);
        }

        if (request.OrganizationalUnitId.HasValue && request.OrganizationalUnitId.Value != DefaultIdType.Empty)
        {
            employee.UpdateOrganizationalUnit(request.OrganizationalUnitId.Value);
        }

        employee.UpdateContactInfo(request.Email, request.PhoneNumber);

        await repository.UpdateAsync(employee, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Employee {EmployeeId} updated successfully", employee.Id);

        return new UpdateEmployeeResponse(employee.Id);
    }
}

