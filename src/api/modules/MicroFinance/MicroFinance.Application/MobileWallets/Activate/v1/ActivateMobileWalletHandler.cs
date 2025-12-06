using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Activate.v1;

public sealed class ActivateMobileWalletHandler(
    [FromKeyedServices("microfinance:mobilewallets")] IRepository<MobileWallet> repository,
    ILogger<ActivateMobileWalletHandler> logger)
    : IRequestHandler<ActivateMobileWalletCommand, ActivateMobileWalletResponse>
{
    public async Task<ActivateMobileWalletResponse> Handle(ActivateMobileWalletCommand request, CancellationToken cancellationToken)
    {
        var wallet = await repository.FirstOrDefaultAsync(new MobileWalletByIdSpec(request.Id), cancellationToken).ConfigureAwait(false)
            ?? throw new Exception($"Mobile wallet {request.Id} not found");

        wallet.Activate();
        await repository.UpdateAsync(wallet, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Activated mobile wallet {Id}", wallet.Id);

        return new ActivateMobileWalletResponse(wallet.Id, wallet.Status);
    }
}
