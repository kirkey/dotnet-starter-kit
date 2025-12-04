using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentAccounts.Invest.v1;

public sealed record InvestCommand(
    Guid Id,
    decimal Amount) : IRequest<InvestResponse>;
