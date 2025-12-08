namespace FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Create.v1;

public sealed record CreateMobileWalletResponse(
    DefaultIdType Id,
    string PhoneNumber,
    string Provider,
    string Status);
