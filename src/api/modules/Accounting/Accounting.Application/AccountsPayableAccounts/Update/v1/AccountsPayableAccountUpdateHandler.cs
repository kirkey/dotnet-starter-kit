using Accounting.Application.AccountsPayableAccounts.Exceptions;
using Accounting.Application.AccountsPayableAccounts.Queries;

namespace Accounting.Application.AccountsPayableAccounts.Update.v1;

/// <summary>
/// Handler for updating an existing accounts payable account.
/// </summary>
public sealed class AccountsPayableAccountUpdateHandler(
    ILogger<AccountsPayableAccountUpdateHandler> logger,
    [FromKeyedServices("accounting")] IRepository<AccountsPayableAccount> repository)
    : IRequestHandler<AccountsPayableAccountUpdateCommand, AccountsPayableAccountUpdateResponse>
{
    public async Task<AccountsPayableAccountUpdateResponse> Handle(AccountsPayableAccountUpdateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var account = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new AccountsPayableAccountNotFoundException(request.Id);

        // Check for duplicate account number if it's being changed
        if (!string.IsNullOrWhiteSpace(request.AccountNumber) && request.AccountNumber != account.AccountNumber)
        {
            var existingByNumber = await repository.FirstOrDefaultAsync(
                new AccountsPayableAccountByNumberSpec(request.AccountNumber), cancellationToken);
            if (existingByNumber != null)
            {
                throw new Exceptions.DuplicateApAccountNumberException(request.AccountNumber);
            }
        }

        account.Update(
            request.AccountNumber,
            request.AccountName,
            request.GeneralLedgerAccountId,
            request.PeriodId,
            request.Description,
            request.Notes,
            request.IsActive);

        await repository.UpdateAsync(account, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Accounts payable account updated {AccountId} - {AccountNumber}", account.Id, account.AccountNumber);
        return new AccountsPayableAccountUpdateResponse(account.Id);
    }
}
