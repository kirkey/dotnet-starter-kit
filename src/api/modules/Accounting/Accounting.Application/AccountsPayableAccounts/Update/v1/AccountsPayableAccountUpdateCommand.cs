namespace Accounting.Application.AccountsPayableAccounts.Update.v1;

/// <summary>
/// Command to update an existing accounts payable account.
/// </summary>
public sealed record AccountsPayableAccountUpdateCommand : IRequest<AccountsPayableAccountUpdateResponse>
{
    /// <summary>
    /// The ID of the accounts payable account to update.
    /// </summary>
    public DefaultIdType Id { get; init; }

    /// <summary>
    /// Account number.
    /// </summary>
    public string? AccountNumber { get; init; }

    /// <summary>
    /// Account name.
    /// </summary>
    public string? AccountName { get; init; }

    /// <summary>
    /// General ledger account reference.
    /// </summary>
    public DefaultIdType? GeneralLedgerAccountId { get; init; }

    /// <summary>
    /// Accounting period reference.
    /// </summary>
    public DefaultIdType? PeriodId { get; init; }

    /// <summary>
    /// Description.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Additional notes.
    /// </summary>
    public string? Notes { get; init; }

    /// <summary>
    /// Whether the account is active.
    /// </summary>
    public bool? IsActive { get; init; }
}

