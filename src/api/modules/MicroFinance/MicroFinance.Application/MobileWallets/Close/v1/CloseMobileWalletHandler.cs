using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Close.v1;

/// <summary>
/// Handler for closing a mobile wallet.
/// </summary>
public sealed class CloseMobileWalletHandler(
    [FromKeyedServices("microfinance:mobilewallets")] IRepository<MobileWallet> repository,
    ILogger<CloseMobileWalletHandler> logger)
    : IRequestHandler<CloseMobileWalletCommand, CloseMobileWalletResponse>
{
    public async Task<CloseMobileWalletResponse> Handle(CloseMobileWalletCommand request, CancellationToken cancellationToken)
    {
        var wallet = await repository.GetByIdAsync(request.MobileWalletId, cancellationToken)
            ?? throw new NotFoundException($"Mobile wallet with ID {request.MobileWalletId} not found.");

        wallet.Close(request.Reason);

        await repository.UpdateAsync(wallet, cancellationToken);
        logger.LogInformation("Closed mobile wallet {MobileWalletId}", request.MobileWalletId);

        return new CloseMobileWalletResponse(wallet.Id, wallet.Status, "Mobile wallet closed successfully.");
    }
}
