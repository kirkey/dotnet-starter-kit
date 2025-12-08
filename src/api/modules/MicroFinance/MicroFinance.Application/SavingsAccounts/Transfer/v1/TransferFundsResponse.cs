namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Transfer.v1;

public sealed record TransferFundsResponse(
    DefaultIdType WithdrawalTransactionId,
    DefaultIdType DepositTransactionId,
    decimal NewSourceBalance,
    decimal NewDestinationBalance);
