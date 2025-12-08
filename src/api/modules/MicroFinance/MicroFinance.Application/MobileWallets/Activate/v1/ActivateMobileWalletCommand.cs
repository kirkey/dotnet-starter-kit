using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Activate.v1;

public sealed record ActivateMobileWalletCommand(DefaultIdType Id) : IRequest<ActivateMobileWalletResponse>;
