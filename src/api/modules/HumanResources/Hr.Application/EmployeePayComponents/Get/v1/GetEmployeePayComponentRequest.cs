namespace FSH.Starter.WebApi.HumanResources.Application.EmployeePayComponents.Get.v1;

public sealed record GetEmployeePayComponentRequest(DefaultIdType Id) : IRequest<EmployeePayComponentResponse>;

