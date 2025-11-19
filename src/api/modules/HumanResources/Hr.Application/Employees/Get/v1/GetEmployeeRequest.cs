namespace FSH.Starter.WebApi.HumanResources.Application.Employees.Get.v1;

public sealed record GetEmployeeRequest(DefaultIdType Id) : IRequest<EmployeeResponse>;

