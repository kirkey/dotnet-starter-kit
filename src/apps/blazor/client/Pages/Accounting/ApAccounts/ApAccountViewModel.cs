namespace FSH.Starter.Blazor.Client.Pages.Accounting.ApAccounts;

/// <summary>
/// View model for AP Account entity.
/// </summary>
public class ApAccountViewModel
{
    /// <summary>
    /// Gets or sets the account number.
    /// </summary>
    public string AccountNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the account name.
    /// </summary>
    public string AccountName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the general ledger account ID.
    /// </summary>
    public DefaultIdType? GeneralLedgerAccountId { get; set; }

    /// <summary>
    /// Gets or sets the period ID.
    /// </summary>
    public DefaultIdType? PeriodId { get; set; }

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the notes.
    /// </summary>
    public string? Notes { get; set; }
}

