namespace FSH.Starter.WebApi.HumanResources.Application.PayrollDeductions.Get.v1;

public sealed record GetPayrollDeductionRequest(DefaultIdType Id) : IRequest<PayrollDeductionResponse>;

