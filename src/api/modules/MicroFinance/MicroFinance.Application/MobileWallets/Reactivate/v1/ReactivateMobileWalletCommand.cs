using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Reactivate.v1;

/// <summary>
/// Command to reactivate a suspended mobile wallet.
/// </summary>
public sealed record ReactivateMobileWalletCommand(DefaultIdType MobileWalletId) : IRequest<ReactivateMobileWalletResponse>;
