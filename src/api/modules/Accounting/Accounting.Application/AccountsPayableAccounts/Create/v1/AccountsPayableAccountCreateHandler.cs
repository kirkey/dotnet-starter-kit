using Accounting.Application.AccountsPayableAccounts.Queries;
using Accounting.Domain.Entities;

namespace Accounting.Application.AccountsPayableAccounts.Create.v1;

/// <summary>
/// Handler for creating a new accounts payable account.
/// </summary>
public sealed class AccountsPayableAccountCreateHandler(
    ILogger<AccountsPayableAccountCreateHandler> logger,
    [FromKeyedServices("accounting")] IRepository<AccountsPayableAccount> repository)
    : IRequestHandler<AccountsPayableAccountCreateCommand, AccountsPayableAccountCreateResponse>
{
    public async Task<AccountsPayableAccountCreateResponse> Handle(AccountsPayableAccountCreateCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Check for duplicate account number
        var existingByNumber = await repository.FirstOrDefaultAsync(
            new AccountsPayableAccountByNumberSpec(request.AccountNumber), cancellationToken);
        if (existingByNumber != null)
        {
            throw new DuplicateAPAccountNumberException(request.AccountNumber);
        }

        var account = AccountsPayableAccount.Create(
            accountNumber: request.AccountNumber,
            accountName: request.AccountName,
            generalLedgerAccountId: request.GeneralLedgerAccountId,
            periodId: request.PeriodId,
            description: request.Description,
            notes: request.Notes);

        await repository.AddAsync(account, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Accounts payable account created {AccountId} - {AccountNumber}", account.Id, account.AccountNumber);
        return new AccountsPayableAccountCreateResponse(account.Id);
    }
}

