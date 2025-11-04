using Accounting.Domain.Constants;
using Accounting.Domain.Events.BankReconciliation;

namespace Accounting.Domain.Entities;

/// <summary>
/// Represents a bank reconciliation process matching bank statements with general ledger cash accounts.
/// </summary>
/// <remarks>
/// Use cases:
/// - Match bank statement transactions with recorded cash receipts and disbursements.
/// - Identify outstanding checks, deposits in transit, and bank errors.
/// - Verify accuracy of cash account balances and detect discrepancies.
/// - Support monthly/periodic reconciliation workflows with approval process.
/// - Track reconciliation items that require investigation or adjustment.
/// - Maintain audit trail of all reconciliation activities and adjustments.
/// - Generate reconciliation reports for management review and compliance.
/// 
/// Default values:
/// - Status: Pending (new reconciliations start in pending status)
/// - IsReconciled: false (becomes true when completed and approved)
/// - ReconciliationDate: required (statement date being reconciled)
/// - StatementBalance: required (ending balance per bank statement)
/// - BookBalance: required (balance per general ledger before adjustments)
/// - AdjustedBalance: calculated (book balance + adjustments)
/// - OutstandingChecksTotal: 0.00 (sum of checks not yet cleared)
/// - DepositsInTransitTotal: 0.00 (sum of deposits not yet on statement)
/// - BankErrors: 0.00 (errors on bank's side)
/// - BookErrors: 0.00 (errors in books requiring adjustment entries)
/// 
/// Business rules:
/// - Reconciliation date cannot be in the future
/// - Cannot modify reconciliation once marked as reconciled
/// - Statement balance and book balance must be provided
/// - Outstanding items must be tracked individually with references
/// - Final adjusted book balance must equal statement balance (after adjustments)
/// - Requires approval workflow before marking as reconciled
/// - Should link to bank account in chart of accounts
/// </remarks>
public class BankReconciliation : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Reference to the bank account in chart of accounts being reconciled.
    /// </summary>
    public DefaultIdType BankAccountId { get; private set; }

    /// <summary>
    /// Date of the bank statement being reconciled.
    /// </summary>
    public DateTime ReconciliationDate { get; private set; }

    /// <summary>
    /// Ending balance per bank statement.
    /// </summary>
    public decimal StatementBalance { get; private set; }

    /// <summary>
    /// Book balance per general ledger before adjustments.
    /// </summary>
    public decimal BookBalance { get; private set; }

    /// <summary>
    /// Adjusted book balance after reconciliation items.
    /// </summary>
    public decimal AdjustedBalance { get; private set; }

    /// <summary>
    /// Total of outstanding checks not yet cleared by bank.
    /// </summary>
    public decimal OutstandingChecksTotal { get; private set; }

    /// <summary>
    /// Total of deposits in transit not yet on bank statement.
    /// </summary>
    public decimal DepositsInTransitTotal { get; private set; }

    /// <summary>
    /// Bank errors requiring correction by the bank.
    /// </summary>
    public decimal BankErrors { get; private set; }

    /// <summary>
    /// Book errors requiring adjustment entries.
    /// </summary>
    public decimal BookErrors { get; private set; }

    /// <summary>
    /// Reconciliation status: Pending, InProgress, Completed, Approved.
    /// Uses string-based values from ReconciliationStatuses constants.
    /// </summary>
    public string Status { get; private set; } = ReconciliationStatuses.Pending;

    /// <summary>
    /// Whether the reconciliation is complete and approved.
    /// </summary>
    public bool IsReconciled { get; private set; }

    /// <summary>
    /// Date when reconciliation was completed.
    /// </summary>
    public DateTime? ReconciledDate { get; private set; }

    /// <summary>
    /// User who completed the reconciliation.
    /// </summary>
    public string? ReconciledBy { get; private set; }

    /// <summary>
    /// User who approved the reconciliation.
    /// </summary>
    public string? ApprovedBy { get; private set; }

    /// <summary>
    /// Date when reconciliation was approved.
    /// </summary>
    public DateTime? ApprovedDate { get; private set; }

    /// <summary>
    /// Optional reference number for the bank statement.
    /// </summary>
    public string? StatementNumber { get; private set; }

    // Description and Notes properties inherited from AuditableEntity base class

    // Parameterless constructor for EF Core
    private BankReconciliation()
    {
    }

    private BankReconciliation(
        DefaultIdType bankAccountId,
        DateTime reconciliationDate,
        decimal statementBalance,
        decimal bookBalance,
        string? statementNumber = null,
        string? description = null,
        string? notes = null)
    {
        if (reconciliationDate > DateTime.UtcNow.Date)
            throw new ArgumentException("Reconciliation date cannot be in the future", nameof(reconciliationDate));

        BankAccountId = bankAccountId;
        ReconciliationDate = reconciliationDate;
        StatementBalance = statementBalance;
        BookBalance = bookBalance;
        AdjustedBalance = bookBalance;
        OutstandingChecksTotal = 0;
        DepositsInTransitTotal = 0;
        BankErrors = 0;
        BookErrors = 0;
        Status = ReconciliationStatuses.Pending;
        IsReconciled = false;
        StatementNumber = statementNumber?.Trim();
        Description = description?.Trim();
        Notes = notes?.Trim();

        QueueDomainEvent(new BankReconciliationCreated(Id, BankAccountId, ReconciliationDate, StatementBalance, BookBalance));
    }

    /// <summary>
    /// Create a new bank reconciliation.
    /// </summary>
    public static BankReconciliation Create(
        DefaultIdType bankAccountId,
        DateTime reconciliationDate,
        decimal statementBalance,
        decimal bookBalance,
        string? statementNumber = null,
        string? description = null,
        string? notes = null)
    {
        return new BankReconciliation(bankAccountId, reconciliationDate, statementBalance, 
            bookBalance, statementNumber, description, notes);
    }

    /// <summary>
    /// Update reconciliation items and recalculate adjusted balance.
    /// </summary>
    public void UpdateReconciliationItems(
        decimal outstandingChecks,
        decimal depositsInTransit,
        decimal bankErrors,
        decimal bookErrors)
    {
        if (IsReconciled)
            throw new BankReconciliationCannotBeModifiedException(Id);

        if (outstandingChecks < 0 || depositsInTransit < 0)
            throw new ArgumentException("Outstanding checks and deposits in transit cannot be negative");

        OutstandingChecksTotal = outstandingChecks;
        DepositsInTransitTotal = depositsInTransit;
        BankErrors = bankErrors;
        BookErrors = bookErrors;

        // Calculate adjusted balance: BookBalance + BookErrors
        AdjustedBalance = BookBalance + BookErrors;

        QueueDomainEvent(new BankReconciliationUpdated(Id, OutstandingChecksTotal, DepositsInTransitTotal, 
            BankErrors, BookErrors, AdjustedBalance));
    }

    /// <summary>
    /// Mark reconciliation as in progress.
    /// </summary>
    public void StartReconciliation()
    {
        if (Status != ReconciliationStatuses.Pending)
            throw new InvalidReconciliationStatusException($"Cannot start reconciliation with status {Status}");

        Status = ReconciliationStatuses.InProgress;
        QueueDomainEvent(new BankReconciliationStarted(Id));
    }

    /// <summary>
    /// Complete the reconciliation process.
    /// </summary>
    public void Complete(string reconciledBy)
    {
        if (IsReconciled)
            throw new BankReconciliationAlreadyReconciledException(Id);

        if (Status != ReconciliationStatuses.InProgress)
            throw new InvalidReconciliationStatusException($"Cannot complete reconciliation with status {Status}");

        // Verify that adjusted book balance matches statement balance (within reasonable tolerance)
        decimal tolerance = 0.01m; // 1 cent tolerance
        decimal expectedBalance = StatementBalance + OutstandingChecksTotal - DepositsInTransitTotal + BankErrors;
        
        if (Math.Abs(AdjustedBalance - expectedBalance) > tolerance)
            throw new ReconciliationBalanceMismatchException(
                $"Adjusted balance {AdjustedBalance:N2} does not match expected balance {expectedBalance:N2}");

        Status = ReconciliationStatuses.Completed;
        ReconciledDate = DateTime.UtcNow;
        ReconciledBy = reconciledBy?.Trim();

        QueueDomainEvent(new BankReconciliationCompleted(Id, reconciledBy ?? string.Empty, ReconciledDate.Value));
    }

    /// <summary>
    /// Approve the completed reconciliation.
    /// </summary>
    public void Approve(string approvedBy)
    {
        if (Status != ReconciliationStatuses.Completed)
            throw new InvalidReconciliationStatusException($"Cannot approve reconciliation with status {Status}");

        if (IsReconciled)
            throw new BankReconciliationAlreadyReconciledException(Id);

        Status = ReconciliationStatuses.Approved;
        IsReconciled = true;
        ApprovedBy = approvedBy?.Trim();
        ApprovedDate = DateTime.UtcNow;

        QueueDomainEvent(new BankReconciliationApproved(Id, approvedBy ?? string.Empty, ApprovedDate.Value));
    }

    /// <summary>
    /// Reject the completed reconciliation for rework.
    /// </summary>
    public void Reject(string rejectedBy, string? reason = null)
    {
        if (Status != ReconciliationStatuses.Completed)
            throw new InvalidReconciliationStatusException($"Cannot reject reconciliation with status {Status}");

        Status = ReconciliationStatuses.Pending;
        Notes = string.IsNullOrWhiteSpace(Notes) 
            ? $"Rejected by {rejectedBy}: {reason}" 
            : $"{Notes}\nRejected by {rejectedBy}: {reason}";

        QueueDomainEvent(new BankReconciliationRejected(Id, rejectedBy, reason));
    }
}

