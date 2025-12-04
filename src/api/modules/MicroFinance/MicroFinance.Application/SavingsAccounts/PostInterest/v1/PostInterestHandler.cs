using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.PostInterest.v1;

/// <summary>
/// Handler for posting interest.
/// </summary>
public sealed class PostInterestHandler(
    IRepository<SavingsAccount> repository,
    ILogger<PostInterestHandler> logger)
    : IRequestHandler<PostInterestCommand, PostInterestResponse>
{
    public async Task<PostInterestResponse> Handle(PostInterestCommand request, CancellationToken cancellationToken)
    {
        var account = await repository.GetByIdAsync(request.AccountId, cancellationToken)
            ?? throw new Exception($"Savings account with ID {request.AccountId} not found.");

        account.PostInterest(request.InterestAmount);

        await repository.UpdateAsync(account, cancellationToken);
        logger.LogInformation("Posted interest {Amount} to account {AccountId}", request.InterestAmount, request.AccountId);

        return new PostInterestResponse(
            account.Id,
            request.InterestAmount,
            account.Balance,
            account.TotalInterestEarned,
            "Interest posted successfully.");
    }
}
