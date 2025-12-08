using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Close.v1;

/// <summary>
/// Command to close a mobile wallet permanently.
/// </summary>
public sealed record CloseMobileWalletCommand(DefaultIdType MobileWalletId, string? Reason = null) : IRequest<CloseMobileWalletResponse>;
