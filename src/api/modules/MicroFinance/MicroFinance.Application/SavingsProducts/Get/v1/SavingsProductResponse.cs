namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsProducts.Get.v1;

public sealed record SavingsProductResponse(
    Guid Id,
    string Code,
    string Name,
    string? Description,
    string CurrencyCode,
    decimal InterestRate,
    string InterestCalculation,
    string InterestPostingFrequency,
    decimal MinOpeningBalance,
    decimal MinBalanceForInterest,
    decimal MinWithdrawalAmount,
    decimal? MaxWithdrawalPerDay,
    bool AllowOverdraft,
    decimal? OverdraftLimit,
    bool IsActive
);
