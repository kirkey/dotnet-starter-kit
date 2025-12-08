namespace FSH.Starter.WebApi.MicroFinance.Application.MobileTransactions.Complete.v1;

public sealed record CompleteMobileTransactionResponse(DefaultIdType Id, string Status, DateTimeOffset CompletedAt);
