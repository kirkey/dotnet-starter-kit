namespace FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Block.v1;

/// <summary>
/// Response after blocking a mobile wallet.
/// </summary>
public sealed record BlockMobileWalletResponse(DefaultIdType MobileWalletId, string Status, string Message);
