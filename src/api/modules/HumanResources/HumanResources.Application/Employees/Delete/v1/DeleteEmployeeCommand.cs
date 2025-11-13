namespace FSH.Starter.WebApi.HumanResources.Application.Employees.Delete.v1;

/// <summary>
/// Command to delete employee.
/// </summary>
public sealed record DeleteEmployeeCommand(DefaultIdType Id) : IRequest<DeleteEmployeeResponse>;

