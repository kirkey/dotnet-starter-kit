using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Suspend.v1;

public sealed record SuspendMobileWalletCommand(DefaultIdType Id, string Reason) : IRequest<SuspendMobileWalletResponse>;
