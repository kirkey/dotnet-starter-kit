namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeEducations.Delete.v1;

/// <summary>
/// Response for deleting an employee education record.
/// </summary>
/// <param name="Id">The identifier of the deleted education record.</param>
public sealed record DeleteEmployeeEducationResponse(DefaultIdType Id);

