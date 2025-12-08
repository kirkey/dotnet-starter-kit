namespace FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Close.v1;

/// <summary>
/// Response after closing a mobile wallet.
/// </summary>
public sealed record CloseMobileWalletResponse(DefaultIdType MobileWalletId, string Status, string Message);
