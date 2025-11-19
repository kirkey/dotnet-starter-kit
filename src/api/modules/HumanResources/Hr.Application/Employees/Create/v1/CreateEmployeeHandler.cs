using FSH.Starter.WebApi.HumanResources.Application.Employees.Specifications;

namespace FSH.Starter.WebApi.HumanResources.Application.Employees.Create.v1;

/// <summary>
/// Handler for creating a new employee with Philippines Labor Code compliance.
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
            throw new DuplicateEmployeeNumberException(request.EmployeeNumber);
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

        // Set hire date if provided
        if (request.HireDate.HasValue)
        {
            employee.SetHireDate(request.HireDate.Value);
        }

        // Set personal information
        if (request.BirthDate.HasValue || !string.IsNullOrWhiteSpace(request.Gender) || !string.IsNullOrWhiteSpace(request.CivilStatus))
        {
            employee.SetPersonalInfo(request.BirthDate, request.Gender, request.CivilStatus);
        }

        // Set government IDs (Philippines mandatory)
        if (!string.IsNullOrWhiteSpace(request.Tin) || 
            !string.IsNullOrWhiteSpace(request.SssNumber) || 
            !string.IsNullOrWhiteSpace(request.PhilHealthNumber) || 
            !string.IsNullOrWhiteSpace(request.PagIbigNumber))
        {
            employee.SetGovernmentIds(
                request.Tin,
                request.SssNumber,
                request.PhilHealthNumber,
                request.PagIbigNumber);
        }

        // Set employment classification
        if (!string.IsNullOrWhiteSpace(request.EmploymentClassification))
        {
            employee.SetEmploymentClassification(request.EmploymentClassification);
        }

        // Regularize if dates provided
        if (request.RegularizationDate.HasValue)
        {
            employee.Regularize(request.RegularizationDate.Value);
        }

        // Set salary if provided
        if (request.BasicMonthlySalary.HasValue)
        {
            employee.SetBasicSalary(request.BasicMonthlySalary.Value);
        }

        // Set PWD status
        if (request.IsPwd)
        {
            employee.SetPwdStatus(request.IsPwd, request.PwdIdNumber);
        }

        // Set Solo Parent status
        if (request.IsSoloParent)
        {
            employee.SetSoloParentStatus(request.IsSoloParent, request.SoloParentIdNumber);
        }

        // Persist to database
        await repository.AddAsync(employee, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Employee created with ID {EmployeeId}, Number {EmployeeNumber}, Name {FullName}",
            employee.Id,
            employee.EmployeeNumber,
            employee.FullName);

        return new CreateEmployeeResponse(employee.Id);
    }
}

