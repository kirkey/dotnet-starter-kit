namespace FSH.Starter.WebApi.HumanResources.Application.Payrolls.Delete.v1;

/// <summary>
/// Command to delete a payroll record.
/// </summary>
public sealed record DeletePayrollCommand(DefaultIdType Id) : IRequest<DeletePayrollResponse>;

