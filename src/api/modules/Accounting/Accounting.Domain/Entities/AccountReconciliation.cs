using Accounting.Domain.Events.AccountReconciliation;

namespace Accounting.Domain.Entities;

/// <summary>
/// Represents a general ledger account reconciliation comparing GL balance with subsidiary ledger.
/// </summary>
/// <remarks>
/// Use cases:
/// - Reconcile GL account balances with subsidiary ledgers (AP, AR, Inventory, etc.)
/// - Identify and track reconciliation discrepancies
/// - Support month-end and period-end close procedures
/// - Maintain audit trail of reconciliation approvals
/// - Enable efficient close and consolidation workflows
/// - Track variance explanations and approvals
/// 
/// Business rules:
/// - Reconciliation variance should be zero for approval
/// - All reconciliation items must be accounted for
/// - Reconciliation requires management approval
/// - Historical reconciliations must be retained for audit
/// - Cannot delete reconciliation once approved
/// </remarks>
public class AccountReconciliation : AuditableEntityWithApproval, IAggregateRoot
{
    /// <summary>
    /// The GL account being reconciled.
    /// </summary>
    public DefaultIdType GeneralLedgerAccountId { get; private set; }

    /// <summary>
    /// The accounting period being reconciled.
    /// </summary>
    public DefaultIdType AccountingPeriodId { get; private set; }

    /// <summary>
    /// The GL account balance at reconciliation date.
    /// </summary>
    public decimal GlBalance { get; private set; }

    /// <summary>
    /// The subsidiary ledger balance (AP, AR, Inventory, etc.)
    /// </summary>
    public decimal SubsidiaryLedgerBalance { get; private set; }

    /// <summary>
    /// Variance between GL and subsidiary ledger (should be zero when reconciled).
    /// </summary>
    public decimal Variance { get; private set; }

    /// <summary>
    /// Reconciliation status: Pending, Reconciled, Adjusted, Approved, Rejected.
    /// </summary>
    public string ReconciliationStatus { get; private set; } = "Pending";

    /// <summary>
    /// Date the reconciliation was performed.
    /// </summary>
    public DateTime ReconciliationDate { get; private set; }

    /// <summary>
    /// Optional explanation of any variance or reconciliation items.
    /// </summary>
    public string? VarianceExplanation { get; private set; }

    /// <summary>
    /// Reference to the source of subsidiary ledger (e.g., "AP Subledger", "AR Subledger").
    /// </summary>
    public string SubsidiaryLedgerSource { get; private set; } = string.Empty;

    /// <summary>
    /// Number of reconciliation line items/exceptions.
    /// </summary>
    public int LineItemCount { get; private set; }

    /// <summary>
    /// Whether adjusting entries have been recorded.
    /// </summary>
    public bool AdjustingEntriesRecorded { get; private set; }

    private readonly List<string> _reconciliationNotes = new();
    /// <summary>
    /// Notes and comments from the reconciliation process.
    /// </summary>
    public IReadOnlyCollection<string> ReconciliationNotes => _reconciliationNotes.AsReadOnly();

    private AccountReconciliation()
    {
        // for EF
    }

    private AccountReconciliation(
        DefaultIdType generalLedgerAccountId,
        DefaultIdType accountingPeriodId,
        decimal glBalance,
        decimal subsidiaryLedgerBalance,
        string subsidiaryLedgerSource,
        DateTime reconciliationDate,
        string? varianceExplanation = null)
    {
        GeneralLedgerAccountId = generalLedgerAccountId;
        AccountingPeriodId = accountingPeriodId;
        GlBalance = glBalance;
        SubsidiaryLedgerBalance = subsidiaryLedgerBalance;
        SubsidiaryLedgerSource = subsidiaryLedgerSource.Trim();
        ReconciliationDate = reconciliationDate;
        VarianceExplanation = varianceExplanation?.Trim();
        Variance = Math.Abs(glBalance - subsidiaryLedgerBalance);
        ReconciliationStatus = "Pending";
        AdjustingEntriesRecorded = false;
        LineItemCount = 0;

        QueueDomainEvent(new AccountReconciliationCreated(
            Id, GeneralLedgerAccountId, AccountingPeriodId, GlBalance, SubsidiaryLedgerBalance, Variance));
    }

    /// <summary>
    /// Create a new account reconciliation.
    /// </summary>
    public static AccountReconciliation Create(
        DefaultIdType generalLedgerAccountId,
        DefaultIdType accountingPeriodId,
        decimal glBalance,
        decimal subsidiaryLedgerBalance,
        string subsidiaryLedgerSource,
        DateTime reconciliationDate,
        string? varianceExplanation = null)
    {
        if (string.IsNullOrWhiteSpace(subsidiaryLedgerSource))
            throw new ArgumentException("Subsidiary ledger source is required.");

        if (glBalance < 0 || subsidiaryLedgerBalance < 0)
            throw new ArgumentException("Balances cannot be negative.");

        return new AccountReconciliation(
            generalLedgerAccountId,
            accountingPeriodId,
            glBalance,
            subsidiaryLedgerBalance,
            subsidiaryLedgerSource,
            reconciliationDate,
            varianceExplanation);
    }

    /// <summary>
    /// Update reconciliation with balances and recalculate variance.
    /// </summary>
    public void UpdateBalances(decimal glBalance, decimal subsidiaryLedgerBalance, string? varianceExplanation = null)
    {
        if (ReconciliationStatus == "Approved")
            throw new InvalidOperationException("Cannot update approved reconciliation.");

        GlBalance = glBalance;
        SubsidiaryLedgerBalance = subsidiaryLedgerBalance;
        Variance = Math.Abs(glBalance - subsidiaryLedgerBalance);
        VarianceExplanation = varianceExplanation?.Trim();

        if (Variance == 0)
            ReconciliationStatus = "Reconciled";

        QueueDomainEvent(new AccountReconciliationUpdated(
            Id, GeneralLedgerAccountId, GlBalance, SubsidiaryLedgerBalance, Variance));
    }

    /// <summary>
    /// Mark reconciliation as having adjusting entries recorded.
    /// </summary>
    public void RecordAdjustingEntries()
    {
        AdjustingEntriesRecorded = true;
        QueueDomainEvent(new AdjustingEntriesRecorded(Id, GeneralLedgerAccountId));
    }

    /// <summary>
    /// Add a note to the reconciliation.
    /// </summary>
    public void AddNote(string note)
    {
        if (!string.IsNullOrWhiteSpace(note))
            _reconciliationNotes.Add(note.Trim());
    }

    /// <summary>
    /// Set the number of line items.
    /// </summary>
    public void SetLineItemCount(int count)
    {
        LineItemCount = count;
    }

    /// <summary>
    /// Approve the reconciliation.
    /// </summary>
    public void Approve(DefaultIdType approverId, string? approverName = null, string? remarks = null)
    {
        if (ReconciliationStatus == "Approved")
            throw new InvalidOperationException("Reconciliation is already approved.");

        Status = "Approved";
        ReconciliationStatus = "Approved";
        ApprovedBy = approverId;
        ApproverName = approverName?.Trim();
        ApprovedOn = DateTime.UtcNow;
        Remarks = remarks?.Trim();

        QueueDomainEvent(new AccountReconciliationApproved(
            Id, GeneralLedgerAccountId, approverId.ToString(), ApprovedOn.Value));
    }

    /// <summary>
    /// Reject the reconciliation.
    /// </summary>
    public void Reject(DefaultIdType rejectedBy, string? reason = null)
    {
        if (ReconciliationStatus == "Approved")
            throw new InvalidOperationException("Cannot reject approved reconciliation.");

        ReconciliationStatus = "Rejected";
        Status = "Rejected";
        ApprovedBy = rejectedBy;
        ApprovedOn = DateTime.UtcNow;
        Remarks = reason?.Trim();

        QueueDomainEvent(new AccountReconciliationRejected(
            Id, GeneralLedgerAccountId, rejectedBy.ToString(), reason));
    }

    /// <summary>
    /// Reopen a reconciliation for corrections.
    /// </summary>
    public void Reopen()
    {
        if (ReconciliationStatus != "Reconciled" && ReconciliationStatus != "Approved")
            throw new InvalidOperationException("Can only reopen reconciled or approved reconciliations.");

        ReconciliationStatus = "Pending";
        Status = "Pending";
        ApprovedOn = null;
        ApprovedBy = null;
        ApproverName = null;

        QueueDomainEvent(new AccountReconciliationReopened(Id, GeneralLedgerAccountId));
    }
}

