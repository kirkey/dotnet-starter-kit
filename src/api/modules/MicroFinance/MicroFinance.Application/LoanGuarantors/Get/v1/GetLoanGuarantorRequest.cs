using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanGuarantors.Get.v1;

public sealed record GetLoanGuarantorRequest(DefaultIdType Id) : IRequest<LoanGuarantorResponse>;
