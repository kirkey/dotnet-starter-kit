namespace FSH.Starter.WebApi.HumanResources.Application.Payrolls.Update.v1;

/// <summary>
/// Response for updating a payroll.
/// </summary>
/// <param name="Id">The identifier of the updated payroll.</param>
public sealed record UpdatePayrollResponse(DefaultIdType Id);

