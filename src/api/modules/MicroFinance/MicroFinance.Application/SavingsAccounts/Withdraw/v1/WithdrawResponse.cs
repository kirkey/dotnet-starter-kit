namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Withdraw.v1;

public sealed record WithdrawResponse(Guid TransactionId, decimal NewBalance);
