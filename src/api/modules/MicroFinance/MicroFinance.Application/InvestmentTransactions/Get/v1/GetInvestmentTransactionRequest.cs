using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentTransactions.Get.v1;

public sealed record GetInvestmentTransactionRequest(DefaultIdType Id) : IRequest<InvestmentTransactionResponse>;
