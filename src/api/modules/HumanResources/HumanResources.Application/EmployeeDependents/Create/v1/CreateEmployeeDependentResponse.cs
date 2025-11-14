namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Create.v1;

/// <summary>
/// Response for creating an employee dependent.
/// </summary>
/// <param name="Id">The identifier of the created dependent.</param>
public sealed record CreateEmployeeDependentResponse(DefaultIdType Id);

