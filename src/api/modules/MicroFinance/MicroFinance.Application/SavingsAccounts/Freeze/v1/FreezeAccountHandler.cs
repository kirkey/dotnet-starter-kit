using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Freeze.v1;

/// <summary>
/// Handler for freezing account.
/// </summary>
public sealed class FreezeAccountHandler(
    [FromKeyedServices("microfinance:savingsaccounts")] IRepository<SavingsAccount> repository,
    ILogger<FreezeAccountHandler> logger)
    : IRequestHandler<FreezeAccountCommand, FreezeAccountResponse>
{
    public async Task<FreezeAccountResponse> Handle(FreezeAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await repository.GetByIdAsync(request.AccountId, cancellationToken)
            ?? throw new NotFoundException($"Savings account with ID {request.AccountId} not found.");

        account.Freeze(request.Reason);

        await repository.UpdateAsync(account, cancellationToken);
        logger.LogInformation("Froze account {AccountId}", request.AccountId);

        return new FreezeAccountResponse(account.Id, account.Status, "Account frozen successfully.");
    }
}
