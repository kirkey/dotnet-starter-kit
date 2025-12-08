using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Deposit.v1;

public sealed record DepositCommand(
    [property: DefaultValue("00000000-0000-0000-0000-000000000000")] DefaultIdType AccountId,
    [property: DefaultValue(1000)] decimal Amount,
    [property: DefaultValue("CASH")] string PaymentMethod,
    [property: DefaultValue("TXN-2024-001")] string? TransactionReference,
    [property: DefaultValue("Regular savings deposit")] string? Notes) : IRequest<DepositResponse>;
