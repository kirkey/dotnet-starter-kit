namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeEducations.Update.v1;

/// <summary>
/// Response for updating an employee education record.
/// </summary>
/// <param name="Id">The identifier of the updated education record.</param>
public sealed record UpdateEmployeeEducationResponse(DefaultIdType Id);

