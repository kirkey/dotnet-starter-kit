namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Get.v1;

/// <summary>
/// Request to get an employee dependent by its identifier.
/// </summary>
public sealed record GetEmployeeDependentRequest(DefaultIdType Id) : IRequest<EmployeeDependentResponse>;

