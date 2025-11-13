namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeContacts.Update.v1;

/// <summary>
/// Command to update employee contact.
/// </summary>
public sealed record UpdateEmployeeContactCommand(
    DefaultIdType Id,
    [property: DefaultValue(null)] string? FirstName = null,
    [property: DefaultValue(null)] string? LastName = null,
    [property: DefaultValue(null)] string? Relationship = null,
    [property: DefaultValue(null)] string? PhoneNumber = null,
    [property: DefaultValue(null)] string? Email = null,
    [property: DefaultValue(null)] string? Address = null,
    [property: DefaultValue(1)] int? Priority = null) : IRequest<UpdateEmployeeContactResponse>;

