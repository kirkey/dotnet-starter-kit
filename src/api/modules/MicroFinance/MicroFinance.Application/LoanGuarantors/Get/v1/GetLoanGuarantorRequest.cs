using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanGuarantors.Get.v1;

public sealed record GetLoanGuarantorRequest(Guid Id) : IRequest<LoanGuarantorResponse>;
