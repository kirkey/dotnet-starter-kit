using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.UpgradeTier.v1;

public sealed class UpgradeTierMobileWalletHandler(
    [FromKeyedServices("microfinance:mobilewallets")] IRepository<MobileWallet> repository,
    ILogger<UpgradeTierMobileWalletHandler> logger)
    : IRequestHandler<UpgradeTierMobileWalletCommand, UpgradeTierMobileWalletResponse>
{
    public async Task<UpgradeTierMobileWalletResponse> Handle(UpgradeTierMobileWalletCommand request, CancellationToken cancellationToken)
    {
        var wallet = await repository.FirstOrDefaultAsync(new MobileWalletByIdSpec(request.Id), cancellationToken).ConfigureAwait(false)
            ?? throw new Exception($"Mobile wallet {request.Id} not found");

        wallet.UpgradeTier(request.NewTier, request.NewDailyLimit, request.NewMonthlyLimit);
        await repository.UpdateAsync(wallet, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Upgraded mobile wallet {Id} to tier {Tier}", wallet.Id, request.NewTier);

        return new UpgradeTierMobileWalletResponse(wallet.Id, wallet.Tier);
    }
}
