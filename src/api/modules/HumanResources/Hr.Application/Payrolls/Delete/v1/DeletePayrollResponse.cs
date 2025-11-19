namespace FSH.Starter.WebApi.HumanResources.Application.Payrolls.Delete.v1;

/// <summary>
/// Response for deleting a payroll.
/// </summary>
/// <param name="Id">The identifier of the deleted payroll.</param>
public sealed record DeletePayrollResponse(DefaultIdType Id);

