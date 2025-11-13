namespace FSH.Starter.WebApi.HumanResources.Application.Employees.Delete.v1;

/// <summary>
/// Handler for deleting employee.
/// </summary>
public sealed class DeleteEmployeeHandler(
    ILogger<DeleteEmployeeHandler> logger,
    [FromKeyedServices("hr:employees")] IRepository<Employee> repository)
    : IRequestHandler<DeleteEmployeeCommand, DeleteEmployeeResponse>
{
    public async Task<DeleteEmployeeResponse> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (employee is null)
        {
            throw new EmployeeNotFoundException(request.Id);
        }

        await repository.DeleteAsync(employee, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Employee {EmployeeId} deleted successfully", employee.Id);

        return new DeleteEmployeeResponse(employee.Id);
    }
}

