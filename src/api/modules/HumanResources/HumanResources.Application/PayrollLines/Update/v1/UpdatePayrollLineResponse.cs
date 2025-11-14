namespace FSH.Starter.WebApi.HumanResources.Application.PayrollLines.Update.v1;

/// <summary>
/// Response for updating a payroll line.
/// </summary>
/// <param name="Id">The identifier of the updated payroll line.</param>
public sealed record UpdatePayrollLineResponse(DefaultIdType Id);

