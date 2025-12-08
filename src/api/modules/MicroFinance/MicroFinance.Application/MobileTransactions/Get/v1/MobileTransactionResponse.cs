namespace FSH.Starter.WebApi.MicroFinance.Application.MobileTransactions.Get.v1;

public sealed record MobileTransactionResponse(
    DefaultIdType Id,
    DefaultIdType WalletId,
    string TransactionReference,
    string TransactionType,
    string Status,
    decimal Amount,
    decimal Fee,
    decimal NetAmount,
    string? SourcePhone,
    string? DestinationPhone,
    DefaultIdType? RecipientWalletId,
    DefaultIdType? LinkedLoanId,
    DefaultIdType? LinkedSavingsAccountId,
    string? ProviderReference,
    DateTimeOffset InitiatedAt,
    DateTimeOffset? CompletedAt,
    string? FailureReason);
