using Accounting.Domain.Events.WriteOff;

namespace Accounting.Domain;

/// <summary>
/// Represents a write-off of uncollectible receivables or bad debts, with approval workflow and journal entry tracking.
/// </summary>
/// <remarks>
/// Use cases:
/// - Write off uncollectible accounts receivable as bad debt expense.
/// - Record collection adjustments and customer account corrections.
/// - Track reasons and approval process for write-off decisions.
/// - Link write-offs to original invoices and customer accounts.
/// - Generate appropriate journal entries for bad debt expense.
/// - Support potential recovery tracking if customer later pays.
/// - Enable aging analysis and collection effectiveness reporting.
/// 
/// Default values:
/// - WriteOffDate: current date (when write-off is initiated)
/// - WriteOffType: BadDebt (most common type)
/// - Status: Pending (requires approval before posting)
/// - Amount: required (amount being written off, must be positive)
/// - IsRecovered: false (becomes true if amount is later collected)
/// - RecoveredAmount: 0.00 (updated if partial recovery occurs)
/// 
/// Business rules:
/// - Write-off amount must be positive
/// - Cannot write off more than the outstanding balance
/// - Requires approval before posting to general ledger
/// - Must link to specific customer and/or invoice
/// - Bad debt expense account must be specified
/// - Write-off reduces accounts receivable balance
/// - Recovery reverses the write-off (fully or partially)
/// - Approval required for amounts above threshold
/// </remarks>
public class WriteOff : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Unique write-off reference number.
    /// </summary>
    public string ReferenceNumber { get; private set; } = string.Empty;

    /// <summary>
    /// Date when write-off is recorded.
    /// </summary>
    public DateTime WriteOffDate { get; private set; }

    /// <summary>
    /// Write-off type: BadDebt, CollectionAdjustment, Discount, Other.
    /// </summary>
    public WriteOffType WriteOffType { get; private set; }

    /// <summary>
    /// Amount being written off.
    /// </summary>
    public decimal Amount { get; private set; }

    /// <summary>
    /// Amount recovered if customer later pays.
    /// </summary>
    public decimal RecoveredAmount { get; private set; }

    /// <summary>
    /// Whether the write-off has been recovered (fully or partially).
    /// </summary>
    public bool IsRecovered { get; private set; }

    /// <summary>
    /// Customer whose account is being written off.
    /// </summary>
    public DefaultIdType? CustomerId { get; private set; }

    /// <summary>
    /// Customer name for display purposes.
    /// </summary>
    public string? CustomerName { get; private set; }

    /// <summary>
    /// Optional specific invoice being written off.
    /// </summary>
    public DefaultIdType? InvoiceId { get; private set; }

    /// <summary>
    /// Invoice number for reference.
    /// </summary>
    public string? InvoiceNumber { get; private set; }

    /// <summary>
    /// Accounts receivable account to credit.
    /// </summary>
    public DefaultIdType ReceivableAccountId { get; private set; }

    /// <summary>
    /// Bad debt expense account to debit.
    /// </summary>
    public DefaultIdType ExpenseAccountId { get; private set; }

    /// <summary>
    /// Optional journal entry created for this write-off.
    /// </summary>
    public DefaultIdType? JournalEntryId { get; private set; }

    /// <summary>
    /// Write-off status: Pending, Approved, Posted, Reversed.
    /// </summary>
    public WriteOffStatus Status { get; private set; }

    /// <summary>
    /// Approval status: Pending, Approved, Rejected.
    /// </summary>
    public ApprovalStatus ApprovalStatus { get; private set; }

    /// <summary>
    /// User who approved the write-off.
    /// </summary>
    public string? ApprovedBy { get; private set; }

    /// <summary>
    /// Date when write-off was approved.
    /// </summary>
    public DateTime? ApprovedDate { get; private set; }

    /// <summary>
    /// Reason for the write-off.
    /// </summary>
    public string? Reason { get; private set; }

    /// <summary>
    /// Optional detailed description.
    /// </summary>
    public new string? Description { get; private set; }

    /// <summary>
    /// Optional notes about collection attempts or circumstances.
    /// </summary>
    public new string? Notes { get; private set; }

    // Parameterless constructor for EF Core
    private WriteOff()
    {
    }

    private WriteOff(
        string referenceNumber,
        DateTime writeOffDate,
        WriteOffType writeOffType,
        decimal amount,
        DefaultIdType receivableAccountId,
        DefaultIdType expenseAccountId,
        DefaultIdType? customerId = null,
        string? customerName = null,
        DefaultIdType? invoiceId = null,
        string? invoiceNumber = null,
        string? reason = null,
        string? description = null,
        string? notes = null)
    {
        if (string.IsNullOrWhiteSpace(referenceNumber))
            throw new ArgumentException("Reference number is required", nameof(referenceNumber));

        if (amount <= 0)
            throw new ArgumentException("Write-off amount must be positive", nameof(amount));

        ReferenceNumber = referenceNumber.Trim();
        WriteOffDate = writeOffDate;
        WriteOffType = writeOffType;
        Amount = amount;
        RecoveredAmount = 0;
        IsRecovered = false;
        CustomerId = customerId;
        CustomerName = customerName?.Trim();
        InvoiceId = invoiceId;
        InvoiceNumber = invoiceNumber?.Trim();
        ReceivableAccountId = receivableAccountId;
        ExpenseAccountId = expenseAccountId;
        Status = WriteOffStatus.Pending;
        ApprovalStatus = ApprovalStatus.Pending;
        Reason = reason?.Trim();
        Description = description?.Trim();
        Notes = notes?.Trim();

        QueueDomainEvent(new WriteOffCreated(Id, ReferenceNumber, WriteOffDate, WriteOffType, Amount));
    }

    /// <summary>
    /// Create a new write-off.
    /// </summary>
    public static WriteOff Create(
        string referenceNumber,
        DateTime writeOffDate,
        WriteOffType writeOffType,
        decimal amount,
        DefaultIdType receivableAccountId,
        DefaultIdType expenseAccountId,
        DefaultIdType? customerId = null,
        string? customerName = null,
        DefaultIdType? invoiceId = null,
        string? invoiceNumber = null,
        string? reason = null,
        string? description = null,
        string? notes = null)
    {
        return new WriteOff(referenceNumber, writeOffDate, writeOffType, amount,
            receivableAccountId, expenseAccountId, customerId, customerName,
            invoiceId, invoiceNumber, reason, description, notes);
    }

    /// <summary>
    /// Update write-off details (only when pending).
    /// </summary>
    public void Update(
        string? reason = null,
        string? description = null,
        string? notes = null)
    {
        if (Status != WriteOffStatus.Pending)
            throw new WriteOffCannotBeModifiedException(Id);

        Reason = reason?.Trim();
        Description = description?.Trim();
        Notes = notes?.Trim();

        QueueDomainEvent(new WriteOffUpdated(Id));
    }

    /// <summary>
    /// Approve the write-off.
    /// </summary>
    public void Approve(string approvedBy)
    {
        if (Status != WriteOffStatus.Pending)
            throw new InvalidWriteOffStatusException($"Cannot approve write-off with status {Status}");

        if (ApprovalStatus == ApprovalStatus.Approved)
            throw new WriteOffAlreadyApprovedException(Id);

        ApprovalStatus = ApprovalStatus.Approved;
        Status = WriteOffStatus.Approved;
        ApprovedBy = approvedBy?.Trim();
        ApprovedDate = DateTime.UtcNow;

        QueueDomainEvent(new WriteOffApproved(Id, approvedBy ?? string.Empty, ApprovedDate.Value));
    }

    /// <summary>
    /// Reject the write-off.
    /// </summary>
    public void Reject(string rejectedBy, string? reason = null)
    {
        if (Status != WriteOffStatus.Pending)
            throw new InvalidWriteOffStatusException($"Cannot reject write-off with status {Status}");

        ApprovalStatus = ApprovalStatus.Rejected;

        if (!string.IsNullOrWhiteSpace(reason))
        {
            Notes = string.IsNullOrWhiteSpace(Notes) 
                ? $"Rejected by {rejectedBy}: {reason}" 
                : $"{Notes}\nRejected by {rejectedBy}: {reason}";
        }

        QueueDomainEvent(new WriteOffRejected(Id, rejectedBy, reason));
    }

    /// <summary>
    /// Post the write-off to general ledger.
    /// </summary>
    public void Post(DefaultIdType journalEntryId)
    {
        if (Status != WriteOffStatus.Approved)
            throw new InvalidWriteOffStatusException($"Cannot post write-off with status {Status}");

        if (ApprovalStatus != ApprovalStatus.Approved)
            throw new WriteOffNotApprovedException(Id);

        Status = WriteOffStatus.Posted;
        JournalEntryId = journalEntryId;

        QueueDomainEvent(new WriteOffPosted(Id, journalEntryId));
    }

    /// <summary>
    /// Record recovery of previously written-off amount.
    /// </summary>
    public void RecordRecovery(decimal recoveryAmount, DefaultIdType? recoveryJournalEntryId = null)
    {
        if (Status != WriteOffStatus.Posted)
            throw new InvalidWriteOffStatusException($"Cannot record recovery for write-off with status {Status}");

        if (recoveryAmount <= 0)
            throw new ArgumentException("Recovery amount must be positive", nameof(recoveryAmount));

        if (RecoveredAmount + recoveryAmount > Amount)
            throw new WriteOffRecoveryExceedsAmountException(Id);

        RecoveredAmount += recoveryAmount;
        IsRecovered = RecoveredAmount >= Amount;

        QueueDomainEvent(new WriteOffRecovered(Id, recoveryAmount, RecoveredAmount, IsRecovered, recoveryJournalEntryId));
    }

    /// <summary>
    /// Reverse the write-off.
    /// </summary>
    public void Reverse(string? reason = null)
    {
        if (Status != WriteOffStatus.Posted)
            throw new InvalidWriteOffStatusException($"Cannot reverse write-off with status {Status}");

        Status = WriteOffStatus.Reversed;

        if (!string.IsNullOrWhiteSpace(reason))
        {
            Notes = string.IsNullOrWhiteSpace(Notes) 
                ? $"Reversed: {reason}" 
                : $"{Notes}\nReversed: {reason}";
        }

        QueueDomainEvent(new WriteOffReversed(Id, reason));
    }
}

/// <summary>
/// Write-off type classifications.
/// </summary>
public enum WriteOffType
{
    BadDebt,
    CollectionAdjustment,
    Discount,
    CustomerCredit,
    Other
}

/// <summary>
/// Write-off status values.
/// </summary>
public enum WriteOffStatus
{
    Pending,
    Approved,
    Posted,
    Reversed
}
