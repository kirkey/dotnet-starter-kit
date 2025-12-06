using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.MobileWallets.LinkSavings.v1;

public sealed class LinkSavingsMobileWalletHandler(
    [FromKeyedServices("microfinance:mobilewallets")] IRepository<MobileWallet> repository,
    ILogger<LinkSavingsMobileWalletHandler> logger)
    : IRequestHandler<LinkSavingsMobileWalletCommand, LinkSavingsMobileWalletResponse>
{
    public async Task<LinkSavingsMobileWalletResponse> Handle(LinkSavingsMobileWalletCommand request, CancellationToken cancellationToken)
    {
        var wallet = await repository.FirstOrDefaultAsync(new MobileWalletByIdSpec(request.Id), cancellationToken).ConfigureAwait(false)
            ?? throw new Exception($"Mobile wallet {request.Id} not found");

        wallet.LinkToSavingsAccount(request.SavingsAccountId);
        await repository.UpdateAsync(wallet, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Linked mobile wallet {WalletId} to savings account {SavingsAccountId}", wallet.Id, request.SavingsAccountId);

        return new LinkSavingsMobileWalletResponse(wallet.Id, wallet.LinkedSavingsAccountId!.Value);
    }
}
