namespace FSH.Starter.Blazor.Client.Pages.Accounting.GeneralLedgers;

/// <summary>
/// ViewModel for General Ledger entries used in the UI.
/// Maps to GeneralLedgerUpdateCommand for edit operations.
/// </summary>
public class GeneralLedgerViewModel
{
    /// <summary>
    /// Primary identifier of the general ledger entry.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Identifier of the source journal entry.
    /// </summary>
    public DefaultIdType EntryId { get; set; }

    /// <summary>
    /// Identifier of the account being posted to.
    /// </summary>
    public DefaultIdType AccountId { get; set; }

    /// <summary>
    /// Account code from chart of accounts (denormalized for display).
    /// </summary>
    public string AccountCode { get; set; } = string.Empty;

    /// <summary>
    /// Account name for display purposes.
    /// </summary>
    public string? AccountName { get; set; }

    /// <summary>
    /// Debit amount (must be non-negative).
    /// </summary>
    public decimal Debit { get; set; }

    /// <summary>
    /// Credit amount (must be non-negative).
    /// </summary>
    public decimal Credit { get; set; }

    /// <summary>
    /// Optional memo text describing the posting.
    /// </summary>
    public string? Memo { get; set; }

    /// <summary>
    /// USOA class for regulatory reporting (Generation, Transmission, Distribution, etc.).
    /// </summary>
    public string? UsoaClass { get; set; }

    /// <summary>
    /// Transaction effective date for this ledger entry.
    /// </summary>
    public DateTime? TransactionDate { get; set; }

    /// <summary>
    /// Optional source reference number (invoice number, check number, etc.).
    /// </summary>
    public string? ReferenceNumber { get; set; }

    /// <summary>
    /// Source type of the transaction (JournalEntry, Invoice, Bill, Payment, etc.).
    /// </summary>
    public string? Source { get; set; }

    /// <summary>
    /// Source document identifier for complete audit trail.
    /// </summary>
    public DefaultIdType? SourceId { get; set; }

    /// <summary>
    /// Indicates whether this entry has been posted to the general ledger.
    /// </summary>
    public bool IsPosted { get; set; }

    /// <summary>
    /// Date when the entry was posted (for audit trail).
    /// </summary>
    public DateTime? PostedDate { get; set; }

    /// <summary>
    /// User who posted the entry (for audit trail and SOX compliance).
    /// </summary>
    public string? PostedBy { get; set; }

    /// <summary>
    /// Optional accounting period identifier.
    /// </summary>
    public DefaultIdType? PeriodId { get; set; }

    /// <summary>
    /// Additional description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Additional notes.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// When the entry was created.
    /// </summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// Who created the entry.
    /// </summary>
    public string? CreatedByUserName { get; set; }

    /// <summary>
    /// Gets the transaction amount (debit or credit, whichever is non-zero).
    /// </summary>
    public decimal Amount => Debit > 0 ? Debit : Credit;

    /// <summary>
    /// Gets the transaction type (Debit or Credit).
    /// </summary>
    public string TransactionType => Debit > 0 ? "Debit" : "Credit";

    /// <summary>
    /// Gets a formatted display string for the entry.
    /// </summary>
    public string DisplayName => $"{AccountCode} - {TransactionDate:yyyy-MM-dd} - {Amount:N2}";
}

