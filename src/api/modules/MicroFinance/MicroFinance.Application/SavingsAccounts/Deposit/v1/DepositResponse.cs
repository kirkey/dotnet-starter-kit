namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Deposit.v1;

public sealed record DepositResponse(DefaultIdType TransactionId, decimal NewBalance);
