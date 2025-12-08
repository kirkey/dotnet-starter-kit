namespace FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Credit.v1;

public sealed record CreditMobileWalletResponse(
    DefaultIdType Id,
    decimal Balance);
