namespace FSH.Starter.WebApi.HumanResources.Application.Employees.Create.v1;

public sealed record CreateEmployeeCommand(
    [property: DefaultValue("EMP-001")] string EmployeeNumber,
    [property: DefaultValue("John")] string FirstName,
    [property: DefaultValue("Doe")] string LastName,
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType OrganizationalUnitId,
    [property: DefaultValue(null)] string? MiddleName = null,
    [property: DefaultValue("john.doe@example.com")] string? Email = null,
    [property: DefaultValue("+1234567890")] string? PhoneNumber = null,
    [property: DefaultValue(null)] DateTime? HireDate = null) : IRequest<CreateEmployeeResponse>;

