namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Transfer.v1;

public sealed record TransferFundsResponse(
    Guid WithdrawalTransactionId,
    Guid DepositTransactionId,
    decimal NewSourceBalance,
    decimal NewDestinationBalance);
