using Accounting.Application.AccountsReceivableAccounts.Queries;
using Accounting.Domain.Entities;

namespace Accounting.Application.AccountsReceivableAccounts.Create.v1;

/// <summary>
/// Handler for creating a new accounts receivable account.
/// </summary>
public sealed class AccountsReceivableAccountCreateHandler(
    ILogger<AccountsReceivableAccountCreateHandler> logger,
    [FromKeyedServices("accounting")] IRepository<AccountsReceivableAccount> repository)
    : IRequestHandler<AccountsReceivableAccountCreateCommand, AccountsReceivableAccountCreateResponse>
{
    public async Task<AccountsReceivableAccountCreateResponse> Handle(AccountsReceivableAccountCreateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Check for duplicate account number
        var existingByNumber = await repository.FirstOrDefaultAsync(
            new AccountsReceivableAccountByNumberSpec(request.AccountNumber), cancellationToken);
        if (existingByNumber != null)
        {
            throw new DuplicateARAccountNumberException(request.AccountNumber);
        }

        var account = AccountsReceivableAccount.Create(
            accountNumber: request.AccountNumber,
            accountName: request.AccountName,
            generalLedgerAccountId: request.GeneralLedgerAccountId,
            periodId: request.PeriodId,
            description: request.Description,
            notes: request.Notes);

        await repository.AddAsync(account, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Accounts receivable account created {AccountId} - {AccountNumber}", account.Id, account.AccountNumber);
        return new AccountsReceivableAccountCreateResponse(account.Id);
    }
}

