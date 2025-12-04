using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.RedeemShares.v1;

/// <summary>
/// Handler for redeeming shares.
/// </summary>
public sealed class RedeemSharesHandler(
    IRepository<ShareAccount> repository,
    ILogger<RedeemSharesHandler> logger)
    : IRequestHandler<RedeemSharesCommand, RedeemSharesResponse>
{
    public async Task<RedeemSharesResponse> Handle(RedeemSharesCommand request, CancellationToken cancellationToken)
    {
        var account = await repository.GetByIdAsync(request.ShareAccountId, cancellationToken)
            ?? throw new Exception($"Share account with ID {request.ShareAccountId} not found.");

        account.RedeemShares(request.NumberOfShares, request.PricePerShare);

        await repository.UpdateAsync(account, cancellationToken);
        logger.LogInformation("Redeemed {Shares} shares from account {AccountId}", request.NumberOfShares, request.ShareAccountId);

        return new RedeemSharesResponse(
            account.Id,
            request.NumberOfShares,
            request.NumberOfShares * request.PricePerShare,
            account.NumberOfShares,
            "Shares redeemed successfully.");
    }
}
