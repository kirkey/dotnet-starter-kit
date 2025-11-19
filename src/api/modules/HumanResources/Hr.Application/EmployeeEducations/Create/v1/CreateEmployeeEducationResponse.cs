namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeEducations.Create.v1;

/// <summary>
/// Response for creating an employee education record.
/// </summary>
/// <param name="Id">The identifier of the created education record.</param>
public sealed record CreateEmployeeEducationResponse(DefaultIdType Id);

