namespace FSH.Starter.WebApi.HumanResources.Application.Employees.Create.v1;

/// <summary>
/// Handler for creating a new employee with Philippines Labor Code compliance.
/// Validates organizational unit existence and sets all mandatory Philippines-specific fields.
/// </summary>
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

        // Validate organizational unit exists
        var organizationalUnit = await organizationalUnitRepository
            .GetByIdAsync(request.OrganizationalUnitId, cancellationToken)
            .ConfigureAwait(false);

        if (organizationalUnit is null)
            throw new OrganizationalUnitNotFoundException(request.OrganizationalUnitId);

        // Create employee with basic information
        var employee = Employee.Create(
            request.EmployeeNumber,
            request.FirstName,
            request.LastName,
            request.OrganizationalUnitId,
            request.MiddleName,
            request.Email,
            request.PhoneNumber);

        // Set hire date
        if (request.HireDate.HasValue)
            employee.SetHireDate(request.HireDate.Value);

        // Philippines-Specific: Set personal information
        if (request.BirthDate.HasValue || !string.IsNullOrWhiteSpace(request.Gender) || !string.IsNullOrWhiteSpace(request.CivilStatus))
        {
            employee.SetPersonalInfo(
                request.BirthDate,
                request.Gender,
                request.CivilStatus);
        }

        // Philippines-Specific: Set government IDs (TIN, SSS, PhilHealth, Pag-IBIG)
        if (!string.IsNullOrWhiteSpace(request.Tin) || !string.IsNullOrWhiteSpace(request.SssNumber) ||
            !string.IsNullOrWhiteSpace(request.PhilHealthNumber) || !string.IsNullOrWhiteSpace(request.PagIbigNumber))
        {
            employee.SetGovernmentIds(
                request.Tin,
                request.SssNumber,
                request.PhilHealthNumber,
                request.PagIbigNumber);
        }

        // Philippines-Specific: Set employment classification per Labor Code Article 280
        if (!string.IsNullOrWhiteSpace(request.EmploymentClassification))
        {
            employee.SetEmploymentClassification(request.EmploymentClassification);
        }

        // Philippines-Specific: Set regularization date if applicable
        if (request.RegularizationDate.HasValue && request.EmploymentClassification == "Regular")
        {
            employee.Regularize(request.RegularizationDate.Value);
        }

        // Philippines-Specific: Set basic monthly salary (for 13th month pay, separation pay)
        if (request.BasicMonthlySalary.HasValue)
        {
            employee.SetBasicSalary(request.BasicMonthlySalary.Value);
        }

        // Philippines-Specific: Set PWD status (RA 7277)
        if (request.IsPwd)
        {
            employee.SetPwdStatus(request.IsPwd, request.PwdIdNumber);
        }

        // Philippines-Specific: Set solo parent status (RA 7305)
        if (request.IsSoloParent)
        {
            employee.SetSoloParentStatus(request.IsSoloParent, request.SoloParentIdNumber);
        }

        await repository.AddAsync(employee, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Employee created with ID {EmployeeId}, Number {EmployeeNumber}, Name {FullName}, Classification {Classification}",
            employee.Id,
            employee.EmployeeNumber,
            employee.FullName,
            employee.EmploymentClassification);

        return new CreateEmployeeResponse(employee.Id);
    }
}

