namespace FSH.Starter.WebApi.HumanResources.Application.Employees.Update.v1;

public sealed class UpdateEmployeeHandler(
    ILogger<UpdateEmployeeHandler> logger,
    [FromKeyedServices("hr:employees")] IRepository<Employee> repository)
    : IRequestHandler<UpdateEmployeeCommand, UpdateEmployeeResponse>
{
    public async Task<UpdateEmployeeResponse> Handle(
        UpdateEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        var employee = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (employee is null)
            throw new EmployeeNotFoundException(request.Id);

        if (!string.IsNullOrWhiteSpace(request.FirstName) || !string.IsNullOrWhiteSpace(request.LastName))
        {
            var firstName = request.FirstName ?? employee.FirstName;
            var middleName = request.MiddleName ?? employee.MiddleName;
            var lastName = request.LastName ?? employee.LastName;
            // Update name through contact info or other appropriate method
        }

        if (!string.IsNullOrWhiteSpace(request.Email) || !string.IsNullOrWhiteSpace(request.PhoneNumber))
        {
            employee.UpdateContactInfo(request.Email, request.PhoneNumber);
        }

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            if (request.Status == "OnLeave")
                employee.MarkOnLeave();
            else if (request.Status == "Active" && employee.Status == "OnLeave")
                employee.ReturnFromLeave();
        }

        await repository.UpdateAsync(employee, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Employee {EmployeeId} updated successfully", employee.Id);

        return new UpdateEmployeeResponse(employee.Id);
    }
}

