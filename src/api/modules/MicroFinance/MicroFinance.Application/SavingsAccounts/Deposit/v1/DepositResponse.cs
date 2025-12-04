namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Deposit.v1;

public sealed record DepositResponse(Guid TransactionId, decimal NewBalance);
