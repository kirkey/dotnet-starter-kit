using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Unfreeze.v1;

/// <summary>
/// Handler for unfreezing account.
/// </summary>
public sealed class UnfreezeAccountHandler(
    IRepository<SavingsAccount> repository,
    ILogger<UnfreezeAccountHandler> logger)
    : IRequestHandler<UnfreezeAccountCommand, UnfreezeAccountResponse>
{
    public async Task<UnfreezeAccountResponse> Handle(UnfreezeAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await repository.GetByIdAsync(request.AccountId, cancellationToken)
            ?? throw new Exception($"Savings account with ID {request.AccountId} not found.");

        account.Unfreeze();

        await repository.UpdateAsync(account, cancellationToken);
        logger.LogInformation("Unfroze account {AccountId}", request.AccountId);

        return new UnfreezeAccountResponse(account.Id, account.Status, "Account unfrozen successfully.");
    }
}
