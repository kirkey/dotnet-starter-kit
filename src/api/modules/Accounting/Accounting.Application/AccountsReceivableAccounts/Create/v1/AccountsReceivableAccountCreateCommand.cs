namespace Accounting.Application.AccountsReceivableAccounts.Create.v1;

/// <summary>
/// Command to create a new accounts receivable account.
/// </summary>
public record AccountsReceivableAccountCreateCommand(
    string AccountNumber,
    string AccountName,
    DefaultIdType? GeneralLedgerAccountId = null,
    DefaultIdType? PeriodId = null,
    string? Description = null,
    string? Notes = null
) : IRequest<AccountsReceivableAccountCreateResponse>;

