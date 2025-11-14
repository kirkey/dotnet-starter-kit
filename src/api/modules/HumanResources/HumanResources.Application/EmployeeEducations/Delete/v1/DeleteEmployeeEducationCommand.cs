namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeEducations.Delete.v1;

/// <summary>
/// Command to delete an employee education record.
/// </summary>
public sealed record DeleteEmployeeEducationCommand(DefaultIdType Id) : IRequest<DeleteEmployeeEducationResponse>;

