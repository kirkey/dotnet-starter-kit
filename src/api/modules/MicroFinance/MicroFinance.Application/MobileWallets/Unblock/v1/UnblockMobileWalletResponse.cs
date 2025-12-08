namespace FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Unblock.v1;

/// <summary>
/// Response after unblocking a mobile wallet.
/// </summary>
public sealed record UnblockMobileWalletResponse(DefaultIdType MobileWalletId, string Status, string Message);
