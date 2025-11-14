namespace FSH.Starter.WebApi.HumanResources.Application.Employees.Update.v1;

public sealed record UpdateEmployeeCommand(
    DefaultIdType Id,
    [property: DefaultValue(null)] string? FirstName = null,
    [property: DefaultValue(null)] string? MiddleName = null,
    [property: DefaultValue(null)] string? LastName = null,
    [property: DefaultValue(null)] string? Email = null,
    [property: DefaultValue(null)] string? PhoneNumber = null,
    [property: DefaultValue(null)] string? Status = null) : IRequest<UpdateEmployeeResponse>;

