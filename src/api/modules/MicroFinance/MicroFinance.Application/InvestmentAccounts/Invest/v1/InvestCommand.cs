using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentAccounts.Invest.v1;

public sealed record InvestCommand(
    DefaultIdType Id,
    decimal Amount) : IRequest<InvestResponse>;
