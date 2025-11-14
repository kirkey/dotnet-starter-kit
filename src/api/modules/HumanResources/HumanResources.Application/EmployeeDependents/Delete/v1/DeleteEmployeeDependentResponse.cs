namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Delete.v1;

/// <summary>
/// Response for deleting an employee dependent.
/// </summary>
/// <param name="Id">The identifier of the deleted dependent.</param>
public sealed record DeleteEmployeeDependentResponse(DefaultIdType Id);

