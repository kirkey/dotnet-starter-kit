using System.ComponentModel.DataAnnotations;

namespace FSH.Starter.Blazor.Client.Pages.Accounting.BankReconciliations;

/// <summary>
/// View model for creating and editing bank reconciliations.
/// </summary>
public class BankReconciliationViewModel
{
    /// <summary>
    /// Reconciliation identifier (null for new reconciliations).
    /// </summary>
    public DefaultIdType? Id { get; set; }

    /// <summary>
    /// Bank account identifier.
    /// </summary>
    [Required(ErrorMessage = "Bank account is required")]
    public DefaultIdType? BankAccountId { get; set; }
    
    /// <summary>
    /// Date of the bank statement being reconciled.
    /// </summary>
    [Required(ErrorMessage = "Reconciliation date is required")]
    public DateTime? ReconciliationDate { get; set; } = DateTime.Today;

    /// <summary>
    /// Ending balance per bank statement.
    /// </summary>
    [Required(ErrorMessage = "Statement balance is required")]
    public decimal StatementBalance { get; set; }

    /// <summary>
    /// Book balance per general ledger before adjustments.
    /// </summary>
    [Required(ErrorMessage = "Book balance is required")]
    public decimal BookBalance { get; set; }

    /// <summary>
    /// Adjusted book balance after reconciliation items.
    /// </summary>
    public decimal AdjustedBalance { get; set; }

    /// <summary>
    /// Total of outstanding checks not yet cleared by bank.
    /// </summary>
    public decimal OutstandingChecksTotal { get; set; }

    /// <summary>
    /// Total of deposits in transit not yet on bank statement.
    /// </summary>
    public decimal DepositsInTransitTotal { get; set; }

    /// <summary>
    /// Bank errors requiring correction by the bank.
    /// </summary>
    public decimal BankErrors { get; set; }

    /// <summary>
    /// Book errors requiring adjustment entries.
    /// </summary>
    public decimal BookErrors { get; set; }

    /// <summary>
    /// Current status of the reconciliation.
    /// </summary>
    public string Status { get; set; } = "Pending";

    /// <summary>
    /// Whether reconciliation is complete and approved.
    /// </summary>
    public bool IsReconciled { get; set; }

    /// <summary>
    /// Date when reconciliation was completed.
    /// </summary>
    public DateTime? ReconciledDate { get; set; }

    /// <summary>
    /// User who completed the reconciliation.
    /// </summary>
    public string? ReconciledBy { get; set; }

    /// <summary>
    /// User who approved the reconciliation.
    /// </summary>
    public string? ApprovedBy { get; set; }

    /// <summary>
    /// Date when reconciliation was approved.
    /// </summary>
    public DateTime? ApprovedDate { get; set; }

    /// <summary>
    /// Optional reference number for the bank statement.
    /// </summary>
    [StringLength(100, ErrorMessage = "Statement number cannot exceed 100 characters")]
    public string? StatementNumber { get; set; }

    /// <summary>
    /// Optional description of the reconciliation.
    /// </summary>
    [StringLength(2048, ErrorMessage = "Description cannot exceed 2048 characters")]
    public string? Description { get; set; }

    /// <summary>
    /// Optional internal notes about the reconciliation.
    /// </summary>
    [StringLength(2048, ErrorMessage = "Notes cannot exceed 2048 characters")]
    public string? Notes { get; set; }

    /// <summary>
    /// Difference between adjusted balance and statement balance.
    /// </summary>
    public decimal Difference => Math.Abs(AdjustedBalance - StatementBalance);

    /// <summary>
    /// Whether the reconciliation is balanced.
    /// </summary>
    public bool IsBalanced => Difference < 0.01m;
}

