namespace FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Get.v1;

public sealed record MobileWalletResponse(
    Guid Id,
    Guid MemberId,
    string PhoneNumber,
    string Provider,
    string? ExternalWalletId,
    string Status,
    string Tier,
    decimal Balance,
    decimal DailyLimit,
    decimal MonthlyLimit,
    decimal DailyUsed,
    decimal MonthlyUsed,
    DateOnly? LastActivityDate,
    bool IsLinkedToBankAccount,
    Guid? LinkedSavingsAccountId);
