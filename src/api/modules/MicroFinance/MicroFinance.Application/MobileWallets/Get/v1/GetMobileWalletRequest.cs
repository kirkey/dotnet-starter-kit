using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Get.v1;

public sealed record GetMobileWalletRequest(Guid Id) : IRequest<MobileWalletResponse>;
