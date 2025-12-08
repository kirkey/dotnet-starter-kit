using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CashVaults.Withdraw.v1;

public sealed record WithdrawCashCommand(
    DefaultIdType Id,
    decimal Amount,
    string? DenominationBreakdown = null) : IRequest<WithdrawCashResponse>;
