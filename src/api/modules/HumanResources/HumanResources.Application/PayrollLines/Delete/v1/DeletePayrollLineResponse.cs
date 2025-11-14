namespace FSH.Starter.WebApi.HumanResources.Application.PayrollLines.Delete.v1;

/// <summary>
/// Response for deleting a payroll line.
/// </summary>
/// <param name="Id">The identifier of the deleted payroll line.</param>
public sealed record DeletePayrollLineResponse(DefaultIdType Id);

