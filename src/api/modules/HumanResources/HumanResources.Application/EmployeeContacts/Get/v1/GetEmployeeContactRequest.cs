namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeContacts.Get.v1;

/// <summary>
/// Request to get employee contact by ID.
/// </summary>
public sealed record GetEmployeeContactRequest(DefaultIdType Id) : IRequest<EmployeeContactResponse>;

