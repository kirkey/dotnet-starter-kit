namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeContacts.Create.v1;

/// <summary>
/// Command to create a new employee contact.
/// </summary>
public sealed record CreateEmployeeContactCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType EmployeeId,
    [property: DefaultValue("John")] string FirstName,
    [property: DefaultValue("Doe")] string LastName,
    [property: DefaultValue("Emergency")] string ContactType,
    [property: DefaultValue("Spouse")] string? Relationship = null,
    [property: DefaultValue("+1234567890")] string? PhoneNumber = null,
    [property: DefaultValue("contact@example.com")] string? Email = null,
    [property: DefaultValue("123 Main St")] string? Address = null) : IRequest<CreateEmployeeContactResponse>;

