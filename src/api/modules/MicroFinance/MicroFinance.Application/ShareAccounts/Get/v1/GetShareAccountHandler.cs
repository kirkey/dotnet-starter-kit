using FSH.Framework.Core.Exceptions;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.Specifications;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.Get.v1;

public sealed class GetShareAccountHandler(
    [FromKeyedServices("microfinance:shareaccounts")] IRepository<ShareAccount> repository)
    : IRequestHandler<GetShareAccountRequest, ShareAccountResponse>
{
    public async Task<ShareAccountResponse> Handle(GetShareAccountRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var shareAccount = await repository.FirstOrDefaultAsync(
            new ShareAccountByIdSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (shareAccount is null)
        {
            throw new NotFoundException($"Share account with ID {request.Id} not found.");
        }

        return new ShareAccountResponse(
            shareAccount.Id,
            shareAccount.AccountNumber,
            shareAccount.MemberId,
            shareAccount.ShareProductId,
            shareAccount.NumberOfShares,
            shareAccount.TotalShareValue,
            shareAccount.TotalPurchases,
            shareAccount.TotalRedemptions,
            shareAccount.TotalDividendsEarned,
            shareAccount.TotalDividendsPaid,
            shareAccount.OpenedDate,
            shareAccount.ClosedDate,
            shareAccount.Status,
            shareAccount.Notes
        );
    }
}
