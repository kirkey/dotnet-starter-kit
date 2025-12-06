using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Suspend.v1;

public sealed class SuspendMobileWalletHandler(
    [FromKeyedServices("microfinance:mobilewallets")] IRepository<MobileWallet> repository,
    ILogger<SuspendMobileWalletHandler> logger)
    : IRequestHandler<SuspendMobileWalletCommand, SuspendMobileWalletResponse>
{
    public async Task<SuspendMobileWalletResponse> Handle(SuspendMobileWalletCommand request, CancellationToken cancellationToken)
    {
        var wallet = await repository.FirstOrDefaultAsync(new MobileWalletByIdSpec(request.Id), cancellationToken).ConfigureAwait(false)
            ?? throw new Exception($"Mobile wallet {request.Id} not found");

        wallet.Suspend(request.Reason);
        await repository.UpdateAsync(wallet, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Suspended mobile wallet {Id} for reason: {Reason}", wallet.Id, request.Reason);

        return new SuspendMobileWalletResponse(wallet.Id, wallet.Status);
    }
}
