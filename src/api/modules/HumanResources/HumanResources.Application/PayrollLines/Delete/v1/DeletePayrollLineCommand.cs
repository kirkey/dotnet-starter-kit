namespace FSH.Starter.WebApi.HumanResources.Application.PayrollLines.Delete.v1;

/// <summary>
/// Command to delete a payroll line.
/// </summary>
public sealed record DeletePayrollLineCommand(DefaultIdType Id) : IRequest<DeletePayrollLineResponse>;

