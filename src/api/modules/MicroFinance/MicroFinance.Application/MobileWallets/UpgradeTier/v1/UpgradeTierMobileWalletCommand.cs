using MediatR;

namespace FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.UpgradeTier.v1;

public sealed record UpgradeTierMobileWalletCommand(
    Guid Id,
    string NewTier,
    decimal NewDailyLimit,
    decimal NewMonthlyLimit) : IRequest<UpgradeTierMobileWalletResponse>;
