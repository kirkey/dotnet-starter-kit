namespace FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Reactivate.v1;

/// <summary>
/// Response after reactivating a mobile wallet.
/// </summary>
public sealed record ReactivateMobileWalletResponse(DefaultIdType MobileWalletId, string Status, string Message);
