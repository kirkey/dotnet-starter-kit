using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Withdraw.v1;

public sealed record WithdrawCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] Guid AccountId,
    [property: DefaultValue(500)] decimal Amount,
    [property: DefaultValue("CASH")] string PaymentMethod,
    [property: DefaultValue("Emergency withdrawal")] string? Notes) : IRequest<WithdrawResponse>;
