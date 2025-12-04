namespace FSH.Starter.WebApi.MicroFinance.Application.MobileTransactions.Get.v1;

public sealed record MobileTransactionResponse(
    Guid Id,
    Guid WalletId,
    string TransactionReference,
    string TransactionType,
    string Status,
    decimal Amount,
    decimal Fee,
    decimal NetAmount,
    string? SourcePhone,
    string? DestinationPhone,
    Guid? RecipientWalletId,
    Guid? LinkedLoanId,
    Guid? LinkedSavingsAccountId,
    string? ProviderReference,
    DateTimeOffset InitiatedAt,
    DateTimeOffset? CompletedAt,
    string? FailureReason);
