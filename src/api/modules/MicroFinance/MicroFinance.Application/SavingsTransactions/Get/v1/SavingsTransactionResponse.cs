namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsTransactions.Get.v1;

public sealed record SavingsTransactionResponse(
    DefaultIdType Id,
    DefaultIdType SavingsAccountId,
    string Reference,
    string TransactionType,
    decimal Amount,
    decimal BalanceAfter,
    DateOnly TransactionDate,
    string? Description,
    string? PaymentMethod
);
