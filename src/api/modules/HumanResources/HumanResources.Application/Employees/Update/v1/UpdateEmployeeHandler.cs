namespace FSH.Starter.WebApi.HumanResources.Application.Employees.Update.v1;

/// <summary>
/// Handler for updating employee information with Philippines Labor Code compliance.
/// Updates only provided fields - supports partial updates.
/// </summary>
public sealed class UpdateEmployeeHandler(
    ILogger<UpdateEmployeeHandler> logger,
    [FromKeyedServices("hr:employees")] IRepository<Employee> repository,
    [FromKeyedServices("hr:organizationalunits")] IReadRepository<OrganizationalUnit> organizationalUnitRepository)
    : IRequestHandler<UpdateEmployeeCommand, UpdateEmployeeResponse>
{
    public async Task<UpdateEmployeeResponse> Handle(
        UpdateEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        var employee = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

        if (employee is null)
            throw new EmployeeNotFoundException(request.Id);

        // Update contact information
        if (!string.IsNullOrWhiteSpace(request.Email) || !string.IsNullOrWhiteSpace(request.PhoneNumber))
        {
            employee.UpdateContactInfo(request.Email, request.PhoneNumber);
        }

        // Philippines-Specific: Update personal information
        if (request.BirthDate.HasValue || !string.IsNullOrWhiteSpace(request.Gender) || !string.IsNullOrWhiteSpace(request.CivilStatus))
        {
            employee.SetPersonalInfo(
                request.BirthDate,
                request.Gender,
                request.CivilStatus);
        }

        // Philippines-Specific: Update government IDs
        if (!string.IsNullOrWhiteSpace(request.Tin) || !string.IsNullOrWhiteSpace(request.SssNumber) ||
            !string.IsNullOrWhiteSpace(request.PhilHealthNumber) || !string.IsNullOrWhiteSpace(request.PagIbigNumber))
        {
            employee.SetGovernmentIds(
                request.Tin,
                request.SssNumber,
                request.PhilHealthNumber,
                request.PagIbigNumber);
        }

        // Philippines-Specific: Update employment classification
        if (!string.IsNullOrWhiteSpace(request.EmploymentClassification))
        {
            employee.SetEmploymentClassification(request.EmploymentClassification);
        }

        // Philippines-Specific: Handle regularization
        if (request.RegularizationDate.HasValue && request.EmploymentClassification == "Regular")
        {
            employee.Regularize(request.RegularizationDate.Value);
        }

        // Philippines-Specific: Update basic monthly salary
        if (request.BasicMonthlySalary.HasValue)
        {
            employee.SetBasicSalary(request.BasicMonthlySalary.Value);
        }

        // Philippines-Specific: Update PWD status
        if (request.IsPwd.HasValue)
        {
            employee.SetPwdStatus(request.IsPwd.Value, request.PwdIdNumber);
        }

        // Philippines-Specific: Update solo parent status
        if (request.IsSoloParent.HasValue)
        {
            employee.SetSoloParentStatus(request.IsSoloParent.Value, request.SoloParentIdNumber);
        }

        // Update organizational unit (transfer)
        if (request.OrganizationalUnitId.HasValue && request.OrganizationalUnitId != employee.OrganizationalUnitId)
        {
            var organizationalUnit = await organizationalUnitRepository
                .GetByIdAsync(request.OrganizationalUnitId.Value, cancellationToken)
                .ConfigureAwait(false);

            if (organizationalUnit is null)
                throw new OrganizationalUnitNotFoundException(request.OrganizationalUnitId.Value);

            employee.UpdateOrganizationalUnit(request.OrganizationalUnitId.Value);
        }

        // Update employment status
        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            if (request.Status == "OnLeave" && employee.Status != "OnLeave")
                employee.MarkOnLeave();
            else if (request.Status == "Active" && employee.Status == "OnLeave")
                employee.ReturnFromLeave();
        }

        await repository.UpdateAsync(employee, cancellationToken).ConfigureAwait(false);

        logger.LogInformation(
            "Employee {EmployeeId} updated successfully. Classification: {Classification}, Status: {Status}",
            employee.Id,
            employee.EmploymentClassification,
            employee.Status);

        return new UpdateEmployeeResponse(employee.Id);
    }
}

