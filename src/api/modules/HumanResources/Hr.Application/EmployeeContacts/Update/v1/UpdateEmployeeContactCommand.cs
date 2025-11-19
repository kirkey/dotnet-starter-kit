namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeContacts.Update.v1;

/// <summary>
/// Command to update an employee contact.
/// </summary>
public sealed record UpdateEmployeeContactCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType Id,
    [property: DefaultValue("John")] string? FirstName = null,
    [property: DefaultValue("Doe")] string? LastName = null,
    [property: DefaultValue("Spouse")] string? Relationship = null,
    [property: DefaultValue("+1234567890")] string? PhoneNumber = null,
    [property: DefaultValue("contact@example.com")] string? Email = null,
    [property: DefaultValue("123 Main St")] string? Address = null,
    [property: DefaultValue(1)] int? Priority = null) : IRequest<UpdateEmployeeContactResponse>;

