namespace Accounting.Application.AccountsPayableAccounts.Create.v1;

/// <summary>
/// Command to create a new accounts payable account.
/// </summary>
public record AccountsPayableAccountCreateCommand(
    string AccountNumber,
    string AccountName,
    DefaultIdType? GeneralLedgerAccountId = null,
    DefaultIdType? PeriodId = null,
    string? Description = null,
    string? Notes = null
) : IRequest<AccountsPayableAccountCreateResponse>;

