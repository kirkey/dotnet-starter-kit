namespace FSH.Starter.WebApi.HumanResources.Application.Employees.Create.v1;

/// <summary>
/// Command to create a new employee.
/// </summary>
public sealed record CreateEmployeeCommand(
    [property: DefaultValue("EMP-001")] string EmployeeNumber,
    [property: DefaultValue("John")] string FirstName,
    [property: DefaultValue("Doe")] string LastName,
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType OrganizationalUnitId,
    [property: DefaultValue(null)] string? MiddleName = null,
    [property: DefaultValue(null)] string? Email = null,
    [property: DefaultValue(null)] string? PhoneNumber = null) : IRequest<CreateEmployeeResponse>;

