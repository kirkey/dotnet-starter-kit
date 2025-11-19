namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeContacts.Get.v1;

/// <summary>
/// Request to get an employee contact by its identifier.
/// </summary>
public sealed record GetEmployeeContactRequest(DefaultIdType Id) : IRequest<EmployeeContactResponse>;

