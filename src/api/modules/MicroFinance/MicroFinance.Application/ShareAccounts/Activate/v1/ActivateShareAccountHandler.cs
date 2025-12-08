using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.ShareAccounts.Activate.v1;

/// <summary>
/// Handler for activating a share account.
/// </summary>
public sealed class ActivateShareAccountHandler(
    [FromKeyedServices("microfinance:shareaccounts")] IRepository<ShareAccount> repository,
    ILogger<ActivateShareAccountHandler> logger)
    : IRequestHandler<ActivateShareAccountCommand, ActivateShareAccountResponse>
{
    public async Task<ActivateShareAccountResponse> Handle(ActivateShareAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await repository.GetByIdAsync(request.ShareAccountId, cancellationToken)
            ?? throw new NotFoundException($"Share account with ID {request.ShareAccountId} not found.");

        account.Activate();

        await repository.UpdateAsync(account, cancellationToken);
        logger.LogInformation("Activated share account {ShareAccountId}", request.ShareAccountId);

        return new ActivateShareAccountResponse(account.Id, account.Status, "Share account activated successfully.");
    }
}
