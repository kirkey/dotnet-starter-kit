namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Create.v1;

public sealed record CreateSavingsAccountResponse(DefaultIdType Id, string AccountNumber, decimal Balance);
