namespace FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.Get.v1;

public sealed record ShareAccountResponse(
    DefaultIdType Id,
    string AccountNumber,
    DefaultIdType MemberId,
    DefaultIdType ShareProductId,
    int NumberOfShares,
    decimal TotalShareValue,
    decimal TotalPurchases,
    decimal TotalRedemptions,
    decimal TotalDividendsEarned,
    decimal TotalDividendsPaid,
    DateOnly OpenedDate,
    DateOnly? ClosedDate,
    string Status,
    string? Notes
);
