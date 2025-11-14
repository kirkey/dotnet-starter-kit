namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeContacts.Delete.v1;

/// <summary>
/// Command to delete an employee contact.
/// </summary>
public sealed record DeleteEmployeeContactCommand(DefaultIdType Id) : IRequest<DeleteEmployeeContactResponse>;

