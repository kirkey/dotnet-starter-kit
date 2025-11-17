using Accounting.Application.AccountsPayableAccounts.Exceptions;

namespace Accounting.Application.AccountsPayableAccounts.Delete.v1;

/// <summary>
/// Handler for deleting an accounts payable account.
/// </summary>
public sealed class AccountsPayableAccountDeleteHandler(
    ILogger<AccountsPayableAccountDeleteHandler> logger,
    [FromKeyedServices("accounting")] IRepository<AccountsPayableAccount> repository)
    : IRequestHandler<AccountsPayableAccountDeleteCommand>
{
    public async Task Handle(AccountsPayableAccountDeleteCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var account = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new AccountsPayableAccountNotFoundException(request.Id);

        // Business rule: Cannot delete if account has an outstanding balance
        if (account.CurrentBalance != 0)
        {
            throw new ApAccountHasOutstandingBalanceException(request.Id);
        }

        await repository.DeleteAsync(account, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Accounts payable account deleted {AccountId} - {AccountNumber}", account.Id, account.AccountNumber);
    }
}

