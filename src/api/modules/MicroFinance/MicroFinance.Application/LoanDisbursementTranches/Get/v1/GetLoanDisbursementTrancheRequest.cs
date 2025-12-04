using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanDisbursementTranches.Get.v1;

public sealed record GetLoanDisbursementTrancheRequest(Guid Id) : IRequest<LoanDisbursementTrancheResponse>;
