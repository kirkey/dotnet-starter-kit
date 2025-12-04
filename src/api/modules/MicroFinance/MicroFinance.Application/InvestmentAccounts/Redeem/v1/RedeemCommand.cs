using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentAccounts.Redeem.v1;

public sealed record RedeemCommand(
    Guid Id,
    decimal Amount,
    decimal GainLoss) : IRequest<RedeemResponse>;
