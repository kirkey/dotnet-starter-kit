namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Get.v1;

/// <summary>
/// Request to get employee dependent by ID.
/// </summary>
public sealed record GetEmployeeDependentRequest(DefaultIdType Id) : IRequest<EmployeeDependentResponse>;

