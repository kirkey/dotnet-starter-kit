namespace FSH.Starter.WebApi.MicroFinance.Application.MobileTransactions.Complete.v1;

public sealed record CompleteMobileTransactionResponse(Guid Id, string Status, DateTimeOffset CompletedAt);
