namespace FSH.Starter.WebApi.HumanResources.Application.Payrolls.Create.v1;

/// <summary>
/// Response for creating a payroll.
/// </summary>
/// <param name="Id">The identifier of the created payroll.</param>
public sealed record CreatePayrollResponse(DefaultIdType Id);

