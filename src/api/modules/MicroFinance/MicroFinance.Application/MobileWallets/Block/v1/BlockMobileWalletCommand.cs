using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Block.v1;

/// <summary>
/// Command to block a mobile wallet for fraud prevention or compliance.
/// </summary>
public sealed record BlockMobileWalletCommand(DefaultIdType MobileWalletId, string Reason) : IRequest<BlockMobileWalletResponse>;
