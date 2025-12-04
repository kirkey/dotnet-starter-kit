using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.Close.v1;

/// <summary>
/// Handler for closing share account.
/// </summary>
public sealed class CloseShareAccountHandler(
    IRepository<ShareAccount> repository,
    ILogger<CloseShareAccountHandler> logger)
    : IRequestHandler<CloseShareAccountCommand, CloseShareAccountResponse>
{
    public async Task<CloseShareAccountResponse> Handle(CloseShareAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await repository.GetByIdAsync(request.AccountId, cancellationToken)
            ?? throw new Exception($"Share account with ID {request.AccountId} not found.");

        account.Close(request.Reason);

        await repository.UpdateAsync(account, cancellationToken);
        logger.LogInformation("Closed share account {AccountId}", request.AccountId);

        return new CloseShareAccountResponse(
            account.Id,
            account.Status,
            account.ClosedDate,
            "Share account closed successfully.");
    }
}
