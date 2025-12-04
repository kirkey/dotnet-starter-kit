using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.PurchaseShares.v1;

public sealed class PurchaseSharesHandler(
    [FromKeyedServices("microfinance:shareaccounts")] IRepository<ShareAccount> repository,
    ILogger<PurchaseSharesHandler> logger)
    : IRequestHandler<PurchaseSharesCommand, PurchaseSharesResponse>
{
    public async Task<PurchaseSharesResponse> Handle(PurchaseSharesCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var shareAccount = await repository.FirstOrDefaultAsync(
            new ShareAccountByIdSpec(request.ShareAccountId), cancellationToken).ConfigureAwait(false);

        if (shareAccount is null)
        {
            throw new NotFoundException($"Share account with ID {request.ShareAccountId} not found.");
        }

        shareAccount.PurchaseShares(request.NumberOfShares, request.PricePerShare);

        await repository.UpdateAsync(shareAccount, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Purchased {NumberOfShares} shares for account {ShareAccountId}", request.NumberOfShares, shareAccount.Id);

        return new PurchaseSharesResponse(shareAccount.Id, shareAccount.NumberOfShares, shareAccount.TotalShareValue);
    }
}
