namespace FSH.Starter.WebApi.HumanResources.Application.EmployeePayComponents.Delete.v1;

public sealed record DeleteEmployeePayComponentCommand(DefaultIdType Id) : IRequest<DeleteEmployeePayComponentResponse>;

