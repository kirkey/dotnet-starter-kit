namespace FSH.Starter.WebApi.HumanResources.Application.Employees.Update.v1;

/// <summary>
/// Command to update employee information.
/// </summary>
public sealed record UpdateEmployeeCommand(
    DefaultIdType Id,
    [property: DefaultValue(null)] string? Email = null,
    [property: DefaultValue(null)] string? PhoneNumber = null,
    [property: DefaultValue(null)] DefaultIdType? OrganizationalUnitId = null) : IRequest<UpdateEmployeeResponse>;

