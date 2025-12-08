using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.CashVaults.Deposit.v1;

public sealed record DepositCashCommand(
    DefaultIdType Id,
    decimal Amount,
    string? DenominationBreakdown = null) : IRequest<DepositCashResponse>;
