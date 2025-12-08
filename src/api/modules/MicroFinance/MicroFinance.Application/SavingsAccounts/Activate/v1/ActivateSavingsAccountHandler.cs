using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using FSH.Framework.Core.Exceptions;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Activate.v1;

/// <summary>
/// Handler for activating a savings account.
/// </summary>
public sealed class ActivateSavingsAccountHandler(
    [FromKeyedServices("microfinance:savingsaccounts")] IRepository<SavingsAccount> repository,
    ILogger<ActivateSavingsAccountHandler> logger)
    : IRequestHandler<ActivateSavingsAccountCommand, ActivateSavingsAccountResponse>
{
    public async Task<ActivateSavingsAccountResponse> Handle(ActivateSavingsAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await repository.GetByIdAsync(request.SavingsAccountId, cancellationToken)
            ?? throw new NotFoundException($"Savings account with ID {request.SavingsAccountId} not found.");

        account.Activate();

        await repository.UpdateAsync(account, cancellationToken);
        logger.LogInformation("Activated savings account {SavingsAccountId}", request.SavingsAccountId);

        return new ActivateSavingsAccountResponse(account.Id, account.Status, "Savings account activated successfully.");
    }
}
