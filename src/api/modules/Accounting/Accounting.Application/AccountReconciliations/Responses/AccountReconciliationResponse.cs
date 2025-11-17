namespace Accounting.Application.AccountReconciliations.Responses;

/// <summary>
/// Response model for account reconciliation.
/// </summary>
public sealed class AccountReconciliationResponse
{
    /// <summary>
    /// Unique identifier.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// GL account being reconciled.
    /// </summary>
    public DefaultIdType GeneralLedgerAccountId { get; set; }

    /// <summary>
    /// Accounting period.
    /// </summary>
    public DefaultIdType AccountingPeriodId { get; set; }

    /// <summary>
    /// GL account balance.
    /// </summary>
    public decimal GlBalance { get; set; }

    /// <summary>
    /// Subsidiary ledger balance.
    /// </summary>
    public decimal SubsidiaryLedgerBalance { get; set; }

    /// <summary>
    /// Variance between balances.
    /// </summary>
    public decimal Variance { get; set; }

    /// <summary>
    /// Reconciliation status.
    /// </summary>
    public string ReconciliationStatus { get; set; } = string.Empty;

    /// <summary>
    /// Date reconciliation was performed.
    /// </summary>
    public DateTime ReconciliationDate { get; set; }

    /// <summary>
    /// Variance explanation.
    /// </summary>
    public string? VarianceExplanation { get; set; }

    /// <summary>
    /// Source of subsidiary ledger.
    /// </summary>
    public string SubsidiaryLedgerSource { get; set; } = string.Empty;

    /// <summary>
    /// Number of line items.
    /// </summary>
    public int LineItemCount { get; set; }

    /// <summary>
    /// Whether adjusting entries were recorded.
    /// </summary>
    public bool AdjustingEntriesRecorded { get; set; }

    /// <summary>
    /// Status of the reconciliation.
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Date created.
    /// </summary>
    public DateTimeOffset CreatedOn { get; set; }

    /// <summary>
    /// Approval information.
    /// </summary>
    public DefaultIdType? ApprovedBy { get; set; }

    /// <summary>
    /// Approver name.
    /// </summary>
    public string? ApproverName { get; set; }

    /// <summary>
    /// Approval date.
    /// </summary>
    public DateTime? ApprovedOn { get; set; }

    /// <summary>
    /// Remarks.
    /// </summary>
    public string? Remarks { get; set; }

    /// <summary>
    /// Reconciliation notes count.
    /// </summary>
    public int NotesCount { get; set; }
}

