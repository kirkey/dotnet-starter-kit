namespace FSH.Starter.WebApi.HumanResources.Application.Employees.Delete.v1;

public sealed record DeleteEmployeeCommand(DefaultIdType Id) : IRequest<DeleteEmployeeResponse>;

