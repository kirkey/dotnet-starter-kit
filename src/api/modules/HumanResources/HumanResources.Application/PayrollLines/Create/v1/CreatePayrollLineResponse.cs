namespace FSH.Starter.WebApi.HumanResources.Application.PayrollLines.Create.v1;

/// <summary>
/// Response for creating a payroll line.
/// </summary>
/// <param name="Id">The identifier of the created payroll line.</param>
public sealed record CreatePayrollLineResponse(DefaultIdType Id);

