using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentAccounts.Get.v1;

public sealed record GetInvestmentAccountRequest(Guid Id) : IRequest<InvestmentAccountResponse>;
