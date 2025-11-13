namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeContacts.Create.v1;

/// <summary>
/// Command to create an employee contact.
/// </summary>
public sealed record CreateEmployeeContactCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType EmployeeId,
    [property: DefaultValue("John")] string FirstName,
    [property: DefaultValue("Doe")] string LastName,
    [property: DefaultValue("Emergency")] string ContactType,
    [property: DefaultValue(null)] string? Relationship = null,
    [property: DefaultValue(null)] string? PhoneNumber = null,
    [property: DefaultValue(null)] string? Email = null,
    [property: DefaultValue(null)] string? Address = null) : IRequest<CreateEmployeeContactResponse>;

