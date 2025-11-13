namespace FSH.Starter.WebApi.HumanResources.Application.Employees.Get.v1;

/// <summary>
/// Request to get employee by ID.
/// </summary>
public sealed record GetEmployeeRequest(DefaultIdType Id) : IRequest<EmployeeResponse>;

