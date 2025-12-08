using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Reactivate.v1;

/// <summary>
/// Handler for reactivating a suspended mobile wallet.
/// </summary>
public sealed class ReactivateMobileWalletHandler(
    [FromKeyedServices("microfinance:mobilewallets")] IRepository<MobileWallet> repository,
    ILogger<ReactivateMobileWalletHandler> logger)
    : IRequestHandler<ReactivateMobileWalletCommand, ReactivateMobileWalletResponse>
{
    public async Task<ReactivateMobileWalletResponse> Handle(ReactivateMobileWalletCommand request, CancellationToken cancellationToken)
    {
        var wallet = await repository.GetByIdAsync(request.MobileWalletId, cancellationToken)
            ?? throw new NotFoundException($"Mobile wallet with ID {request.MobileWalletId} not found.");

        wallet.Reactivate();

        await repository.UpdateAsync(wallet, cancellationToken);
        logger.LogInformation("Reactivated mobile wallet {MobileWalletId}", request.MobileWalletId);

        return new ReactivateMobileWalletResponse(wallet.Id, wallet.Status, "Mobile wallet reactivated successfully.");
    }
}
