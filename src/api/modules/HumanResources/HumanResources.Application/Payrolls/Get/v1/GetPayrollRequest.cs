namespace FSH.Starter.WebApi.HumanResources.Application.Payrolls.Get.v1;

/// <summary>
/// Request to get a payroll by its identifier.
/// </summary>
public sealed record GetPayrollRequest(DefaultIdType Id) : IRequest<PayrollResponse>;

