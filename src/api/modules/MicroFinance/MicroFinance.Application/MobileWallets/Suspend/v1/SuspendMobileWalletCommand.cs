using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Suspend.v1;

public sealed record SuspendMobileWalletCommand(Guid Id, string Reason) : IRequest<SuspendMobileWalletResponse>;
