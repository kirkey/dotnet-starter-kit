using FSH.Starter.WebApi.HumanResources.Application.Employees.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.Employees.Get.v1;

/// <summary>
/// Handler for getting employee by ID.
/// </summary>
public sealed class GetEmployeeHandler(
    [FromKeyedServices("hr:employees")] IReadRepository<Employee> repository)
    : IRequestHandler<GetEmployeeRequest, EmployeeResponse>
{
    public async Task<EmployeeResponse> Handle(GetEmployeeRequest request, CancellationToken cancellationToken)
    {
        var employee = await repository
            .FirstOrDefaultAsync(new EmployeeByIdSpec(request.Id), cancellationToken)
            .ConfigureAwait(false);

        if (employee is null)
        {
            throw new EmployeeNotFoundException(request.Id);
        }

        return new EmployeeResponse
        {
            Id = employee.Id,
            EmployeeNumber = employee.EmployeeNumber,
            FirstName = employee.FirstName,
            MiddleName = employee.MiddleName,
            LastName = employee.LastName,
            FullName = employee.FullName,
            OrganizationalUnitId = employee.OrganizationalUnitId,
            OrganizationalUnitName = employee.OrganizationalUnit?.Name,
            Email = employee.Email,
            PhoneNumber = employee.PhoneNumber,
            HireDate = employee.HireDate,
            Status = employee.Status,
            TerminationDate = employee.TerminationDate,
            TerminationReason = employee.TerminationReason,
            IsActive = employee.IsActive
        };
    }
}

