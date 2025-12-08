using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanDisbursementTranches.Get.v1;

public sealed record GetLoanDisbursementTrancheRequest(DefaultIdType Id) : IRequest<LoanDisbursementTrancheResponse>;
