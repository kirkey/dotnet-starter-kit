namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeContacts.Delete.v1;

/// <summary>
/// Command to delete employee contact.
/// </summary>
public sealed record DeleteEmployeeContactCommand(DefaultIdType Id) : IRequest<DeleteEmployeeContactResponse>;

