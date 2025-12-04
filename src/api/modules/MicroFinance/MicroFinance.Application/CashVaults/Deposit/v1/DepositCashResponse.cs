namespace FSH.Starter.WebApi.MicroFinance.Application.CashVaults.Deposit.v1;

public sealed record DepositCashResponse(Guid Id, decimal PreviousBalance, decimal NewBalance, decimal Amount);
