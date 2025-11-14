using FSH.Starter.WebApi.HumanResources.Application.EmployeeEducations.Get.v1;

namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeEducations.Get.v1;

/// <summary>
/// Request to get an employee education record by its identifier.
/// </summary>
public sealed record GetEmployeeEducationRequest(DefaultIdType Id) : IRequest<EmployeeEducationResponse>;

