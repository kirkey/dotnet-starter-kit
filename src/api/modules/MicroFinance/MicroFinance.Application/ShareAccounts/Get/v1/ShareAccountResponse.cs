namespace FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.Get.v1;

public sealed record ShareAccountResponse(
    Guid Id,
    string AccountNumber,
    Guid MemberId,
    Guid ShareProductId,
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
