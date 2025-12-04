namespace FSH.Starter.WebApi.MicroFinance.Application.MobileTransactions.Fail.v1;

public sealed record FailMobileTransactionResponse(Guid Id, string Status, string FailureReason);
