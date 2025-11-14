namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeContacts.Delete.v1;

/// <summary>
/// Response for deleting an employee contact.
/// </summary>
/// <param name="Id">The identifier of the deleted contact.</param>
public sealed record DeleteEmployeeContactResponse(DefaultIdType Id);

