namespace FSH.Starter.WebApi.MicroFinance.Application.CashVaults.Withdraw.v1;

public sealed record WithdrawCashResponse(DefaultIdType Id, decimal PreviousBalance, decimal NewBalance, decimal Amount);
