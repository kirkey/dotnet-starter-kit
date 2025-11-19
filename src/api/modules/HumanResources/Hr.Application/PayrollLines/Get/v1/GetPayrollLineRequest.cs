namespace FSH.Starter.WebApi.HumanResources.Application.PayrollLines.Get.v1;

/// <summary>
/// Request to get a payroll line by its identifier.
/// </summary>
public sealed record GetPayrollLineRequest(DefaultIdType Id) : IRequest<PayrollLineResponse>;

