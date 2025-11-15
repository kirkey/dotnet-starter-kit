using FSH.Starter.WebApi.HumanResources.Application.Employees.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.Employees.Get.v1;

/// <summary>
/// Handler for getting employee details by ID.
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
            Email = employee.Email,
            PhoneNumber = employee.PhoneNumber,
            HireDate = employee.HireDate,
            Status = employee.Status,
            BirthDate = employee.BirthDate,
            Gender = employee.Gender,
            CivilStatus = employee.CivilStatus,
            Tin = employee.Tin,
            SssNumber = employee.SssNumber,
            PhilHealthNumber = employee.PhilHealthNumber,
            PagIbigNumber = employee.PagIbigNumber,
            EmploymentClassification = employee.EmploymentClassification,
            RegularizationDate = employee.RegularizationDate,
            BasicMonthlySalary = employee.BasicMonthlySalary,
            TerminationDate = employee.TerminationDate,
            TerminationReason = employee.TerminationReason,
            TerminationMode = employee.TerminationMode,
            SeparationPayBasis = employee.SeparationPayBasis,
            SeparationPayAmount = employee.SeparationPayAmount,
            IsPwd = employee.IsPwd,
            PwdIdNumber = employee.PwdIdNumber,
            IsSoloParent = employee.IsSoloParent,
            SoloParentIdNumber = employee.SoloParentIdNumber,
            IsActive = employee.IsActive
        };
    }
}

