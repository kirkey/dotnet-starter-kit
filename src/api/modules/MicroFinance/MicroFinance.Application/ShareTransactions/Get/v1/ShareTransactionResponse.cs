namespace FSH.Starter.WebApi.MicroFinance.Application.ShareTransactions.Get.v1;

public sealed record ShareTransactionResponse(
    Guid Id,
    Guid ShareAccountId,
    string Reference,
    string TransactionType,
    int NumberOfShares,
    decimal PricePerShare,
    decimal TotalAmount,
    int SharesBalanceAfter,
    DateOnly TransactionDate,
    string? Description,
    string? PaymentMethod
);
