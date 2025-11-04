namespace Accounting.Application.BankReconciliations.Responses;

/// <summary>
/// Response DTO for bank reconciliation data.
/// Contains all reconciliation details including status, balances, adjustments, and audit information.
/// </summary>
public class BankReconciliationResponse
{
    /// <summary>
    /// Unique identifier for the bank reconciliation.
    /// </summary>
    public DefaultIdType Id { get; set; }

    /// <summary>
    /// Reference to the bank account being reconciled.
    /// </summary>
    public DefaultIdType BankAccountId { get; set; }

    /// <summary>
    /// Date of the bank statement being reconciled.
    /// </summary>
    public DateTime ReconciliationDate { get; set; }

    /// <summary>
    /// Ending balance per bank statement.
    /// </summary>
    public decimal StatementBalance { get; set; }

    /// <summary>
    /// Book balance per general ledger before adjustments.
    /// </summary>
    public decimal BookBalance { get; set; }

    /// <summary>
    /// Adjusted book balance after all reconciliation items and errors.
    /// Calculated as: BookBalance + BookErrors
    /// </summary>
    public decimal AdjustedBalance { get; set; }

    /// <summary>
    /// Total of outstanding checks not yet cleared by the bank.
    /// </summary>
    public decimal OutstandingChecksTotal { get; set; }

    /// <summary>
    /// Total of deposits in transit not yet shown on bank statement.
    /// </summary>
    public decimal DepositsInTransitTotal { get; set; }

    /// <summary>
    /// Bank errors requiring correction by the financial institution.
    /// </summary>
    public decimal BankErrors { get; set; }

    /// <summary>
    /// Book errors requiring adjustment entries to the general ledger.
    /// </summary>
    public decimal BookErrors { get; set; }

    /// <summary>
    /// Current reconciliation status: Pending, InProgress, Completed, or Approved.
    /// Uses string-based values for flexibility and database-level filtering.
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether the reconciliation is complete and approved.
    /// True when Status = Approved.
    /// </summary>
    public bool IsReconciled { get; set; }

    /// <summary>
    /// Date when the reconciliation was marked as completed.
    /// </summary>
    public DateTime? ReconciledDate { get; set; }

    /// <summary>
    /// User identifier or name who completed the reconciliation.
    /// </summary>
    public string? ReconciledBy { get; set; }

    /// <summary>
    /// User identifier or name who approved the completed reconciliation.
    /// </summary>
    public string? ApprovedBy { get; set; }

    /// <summary>
    /// Date when the reconciliation was approved.
    /// </summary>
    public DateTime? ApprovedDate { get; set; }

    /// <summary>
    /// Optional reference number from the bank statement for reference.
    /// </summary>
    public string? StatementNumber { get; set; }

    /// <summary>
    /// Optional description of the reconciliation purpose or scope.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Optional internal notes about the reconciliation process, findings, or issues.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// UTC timestamp when the reconciliation was created.
    /// </summary>
    public DateTimeOffset CreatedOn { get; set; }

    /// <summary>
    /// User identifier who created the reconciliation.
    /// </summary>
    public DefaultIdType? CreatedBy { get; set; }

    /// <summary>
    /// UTC timestamp when the reconciliation was last modified.
    /// </summary>
    public DateTimeOffset? LastModifiedOn { get; set; }

    /// <summary>
    /// User identifier who last modified the reconciliation.
    /// </summary>
    public DefaultIdType? LastModifiedBy { get; set; }
}
