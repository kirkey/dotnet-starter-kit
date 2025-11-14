namespace FSH.Starter.WebApi.HumanResources.Application.EmployeeDependents.Delete.v1;

/// <summary>
/// Command to delete an employee dependent.
/// </summary>
public sealed record DeleteEmployeeDependentCommand(DefaultIdType Id) : IRequest<DeleteEmployeeDependentResponse>;

