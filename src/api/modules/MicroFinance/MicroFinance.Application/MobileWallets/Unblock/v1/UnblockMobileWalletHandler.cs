using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Unblock.v1;

/// <summary>
/// Handler for unblocking a mobile wallet.
/// </summary>
public sealed class UnblockMobileWalletHandler(
    [FromKeyedServices("microfinance:mobilewallets")] IRepository<MobileWallet> repository,
    ILogger<UnblockMobileWalletHandler> logger)
    : IRequestHandler<UnblockMobileWalletCommand, UnblockMobileWalletResponse>
{
    public async Task<UnblockMobileWalletResponse> Handle(UnblockMobileWalletCommand request, CancellationToken cancellationToken)
    {
        var wallet = await repository.GetByIdAsync(request.MobileWalletId, cancellationToken)
            ?? throw new NotFoundException($"Mobile wallet with ID {request.MobileWalletId} not found.");

        wallet.Unblock();

        await repository.UpdateAsync(wallet, cancellationToken);
        logger.LogInformation("Unblocked mobile wallet {MobileWalletId}", request.MobileWalletId);

        return new UnblockMobileWalletResponse(wallet.Id, wallet.Status, "Mobile wallet unblocked successfully.");
    }
}
