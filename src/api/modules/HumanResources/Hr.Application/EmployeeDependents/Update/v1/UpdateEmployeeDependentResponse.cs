namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Update.v1;

/// <summary>
/// Response for updating an employee dependent.
/// </summary>
/// <param name="Id">The identifier of the updated dependent.</param>
public sealed record UpdateEmployeeDependentResponse(DefaultIdType Id);

