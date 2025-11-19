namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeContacts.Update.v1;

/// <summary>
/// Response for updating an employee contact.
/// </summary>
/// <param name="Id">The identifier of the updated contact.</param>
public sealed record UpdateEmployeeContactResponse(DefaultIdType Id);

