using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.MicroFinance.Domain;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.WebApi.MicroFinance.Application.SavingsAccounts.Close.v1;

/// <summary>
/// Handler for closing account.
/// </summary>
public sealed class CloseAccountHandler(
    IRepository<SavingsAccount> repository,
    ILogger<CloseAccountHandler> logger)
    : IRequestHandler<CloseAccountCommand, CloseAccountResponse>
{
    public async Task<CloseAccountResponse> Handle(CloseAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await repository.GetByIdAsync(request.AccountId, cancellationToken)
            ?? throw new Exception($"Savings account with ID {request.AccountId} not found.");

        account.Close(request.Reason);

        await repository.UpdateAsync(account, cancellationToken);
        logger.LogInformation("Closed account {AccountId}", request.AccountId);

        return new CloseAccountResponse(account.Id, account.Status, account.ClosedDate, "Account closed successfully.");
    }
}
