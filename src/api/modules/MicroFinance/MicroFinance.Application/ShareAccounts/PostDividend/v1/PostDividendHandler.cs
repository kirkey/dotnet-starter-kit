using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.PostDividend.v1;

/// <summary>
/// Handler for posting dividend.
/// </summary>
public sealed class PostDividendHandler(
    IRepository<ShareAccount> repository,
    ILogger<PostDividendHandler> logger)
    : IRequestHandler<PostDividendCommand, PostDividendResponse>
{
    public async Task<PostDividendResponse> Handle(PostDividendCommand request, CancellationToken cancellationToken)
    {
        var account = await repository.GetByIdAsync(request.ShareAccountId, cancellationToken)
            ?? throw new Exception($"Share account with ID {request.ShareAccountId} not found.");

        account.PostDividend(request.DividendAmount);

        await repository.UpdateAsync(account, cancellationToken);
        logger.LogInformation("Posted dividend {Amount} to share account {AccountId}", request.DividendAmount, request.ShareAccountId);

        return new PostDividendResponse(
            account.Id,
            request.DividendAmount,
            account.TotalDividendsEarned,
            "Dividend posted successfully.");
    }
}
