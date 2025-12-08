using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Block.v1;

/// <summary>
/// Handler for blocking a mobile wallet.
/// </summary>
public sealed class BlockMobileWalletHandler(
    [FromKeyedServices("microfinance:mobilewallets")] IRepository<MobileWallet> repository,
    ILogger<BlockMobileWalletHandler> logger)
    : IRequestHandler<BlockMobileWalletCommand, BlockMobileWalletResponse>
{
    public async Task<BlockMobileWalletResponse> Handle(BlockMobileWalletCommand request, CancellationToken cancellationToken)
    {
        var wallet = await repository.GetByIdAsync(request.MobileWalletId, cancellationToken)
            ?? throw new NotFoundException($"Mobile wallet with ID {request.MobileWalletId} not found.");

        wallet.Block(request.Reason);

        await repository.UpdateAsync(wallet, cancellationToken);
        logger.LogInformation("Blocked mobile wallet {MobileWalletId}. Reason: {Reason}", request.MobileWalletId, request.Reason);

        return new BlockMobileWalletResponse(wallet.Id, wallet.Status, "Mobile wallet blocked successfully.");
    }
}
