using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Unblock.v1;

/// <summary>
/// Command to unblock a blocked mobile wallet.
/// </summary>
public sealed record UnblockMobileWalletCommand(DefaultIdType MobileWalletId) : IRequest<UnblockMobileWalletResponse>;
