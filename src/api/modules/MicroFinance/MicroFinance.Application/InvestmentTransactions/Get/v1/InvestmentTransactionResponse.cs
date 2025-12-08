namespace FSH.Starter.WebApi.MicroFinance.Application.InvestmentTransactions.Get.v1;

public sealed record InvestmentTransactionResponse(
    DefaultIdType Id,
    DefaultIdType InvestmentAccountId,
    DefaultIdType ProductId,
    string TransactionReference,
    string TransactionType,
    string Status,
    decimal Amount,
    decimal? Units,
    decimal? NavAtTransaction,
    decimal? EntryLoadAmount,
    decimal? ExitLoadAmount,
    decimal NetAmount,
    decimal? GainLoss,
    DateTimeOffset RequestedAt,
    DateTimeOffset? ProcessedAt,
    DateOnly? AllotmentDate,
    string? PaymentMode,
    string? PaymentReference,
    string? Notes,
    string? FailureReason);
