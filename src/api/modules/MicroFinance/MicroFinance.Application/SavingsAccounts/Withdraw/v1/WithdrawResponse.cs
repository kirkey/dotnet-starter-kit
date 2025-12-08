namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Withdraw.v1;

public sealed record WithdrawResponse(DefaultIdType TransactionId, decimal NewBalance);
