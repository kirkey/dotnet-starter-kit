namespace FSH.Starter.WebApi.MicroFinance.Application.CashVaults.Withdraw.v1;

public sealed record WithdrawCashResponse(Guid Id, decimal PreviousBalance, decimal NewBalance, decimal Amount);
