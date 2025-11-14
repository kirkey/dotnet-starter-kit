namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeContacts.Create.v1;

/// <summary>
/// Response for creating an employee contact.
/// </summary>
/// <param name="Id">The identifier of the created contact.</param>
public sealed record CreateEmployeeContactResponse(DefaultIdType Id);

