using Accounting.Domain.Events.DebitMemo;

namespace Accounting.Domain;

/// <summary>
/// Represents a debit memo used to increase a customer's receivable balance or vendor's payable balance.
/// </summary>
/// <remarks>
/// Use cases:
/// - Record additional charges to customers after invoice has been sent (underbilling corrections).
/// - Adjust vendor payables upward for additional costs or charges.
/// - Document price increases or additional fees discovered post-invoice.
/// - Handle service upgrades or additional work performed but not initially billed.
/// - Support billing corrections and adjustments with proper audit trail.
/// - Enable integration with accounts receivable and payable aging reports.
/// 
/// Default values:
/// - MemoNumber: required unique identifier (example: "DM-2025-001")
/// - MemoDate: required date when memo is issued (example: 2025-10-02)
/// - Amount: required positive amount (example: 500.00)
/// - Status: "Draft" (new memos start as draft)
/// - IsApplied: false (memo starts as unapplied)
/// - AppliedDate: null (set when applied to an invoice/bill)
/// - ReferenceType: "Customer" or "Vendor"
/// - ReferenceId: customer or vendor identifier
/// 
/// Business rules:
/// - Amount must be positive
/// - Cannot modify after approval
/// - Cannot apply unless approved
/// - Can be applied to multiple invoices/bills
/// - Must have proper authorization before approval
/// - Voiding reverses any applications
/// </remarks>
/// <seealso cref="Accounting.Domain.Events.DebitMemo.DebitMemoCreated"/>
/// <seealso cref="Accounting.Domain.Events.DebitMemo.DebitMemoUpdated"/>
/// <seealso cref="Accounting.Domain.Events.DebitMemo.DebitMemoApproved"/>
/// <seealso cref="Accounting.Domain.Events.DebitMemo.DebitMemoApplied"/>
/// <seealso cref="Accounting.Domain.Events.DebitMemo.DebitMemoVoided"/>
public class DebitMemo : AuditableEntity, IAggregateRoot
{
    private const int MaxMemoNumberLength = 50;
    private const int MaxReferenceTypeLength = 20;
    private const int MaxReasonLength = 500;

    /// <summary>
    /// Unique memo number for tracking and reference.
    /// Example: "DM-2025-001", "DEBIT-OCT-12345"
    /// </summary>
    public string MemoNumber { get; private set; }

    /// <summary>
    /// Date the memo was issued.
    /// </summary>
    public DateTime MemoDate { get; private set; }

    /// <summary>
    /// Amount of the debit adjustment; must be positive.
    /// Example: 500.00 for additional charges
    /// </summary>
    public decimal Amount { get; private set; }

    /// <summary>
    /// Amount already applied to invoices/bills.
    /// Default: 0.00
    /// </summary>
    public decimal AppliedAmount { get; private set; }

    /// <summary>
    /// Remaining unapplied amount available.
    /// Calculated as Amount - AppliedAmount
    /// </summary>
    public decimal UnappliedAmount => Amount - AppliedAmount;

    /// <summary>
    /// Type of reference: "Customer" or "Vendor"
    /// </summary>
    public string ReferenceType { get; private set; }

    /// <summary>
    /// Identifier of the customer or vendor this memo applies to.
    /// </summary>
    public DefaultIdType ReferenceId { get; private set; }

    /// <summary>
    /// Optional reference to original invoice or bill being adjusted.
    /// </summary>
    public DefaultIdType? OriginalDocumentId { get; private set; }

    /// <summary>
    /// Reason for the debit memo.
    /// Example: "Additional charges for emergency service call", "Price adjustment per contract amendment"
    /// </summary>
    public string? Reason { get; private set; }

    /// <summary>
    /// Current status: Draft, Approved, Applied, Voided
    /// </summary>
    public string Status { get; private set; }

    /// <summary>
    /// Whether the memo has been applied to invoices/bills.
    /// </summary>
    public bool IsApplied { get; private set; }

    /// <summary>
    /// Date when the memo was applied.
    /// </summary>
    public DateTime? AppliedDate { get; private set; }

    /// <summary>
    /// Approval status: Pending, Approved, Rejected
    /// </summary>
    public string ApprovalStatus { get; private set; }

    /// <summary>
    /// Person who approved the memo.
    /// </summary>
    public string? ApprovedBy { get; private set; }

    /// <summary>
    /// Date when the memo was approved.
    /// </summary>
    public DateTime? ApprovedDate { get; private set; }

    private DebitMemo()
    {
        MemoNumber = string.Empty;
        ReferenceType = string.Empty;
        Status = "Draft";
        ApprovalStatus = "Pending";
    }

    private DebitMemo(string memoNumber, DateTime memoDate, decimal amount, string referenceType,
        DefaultIdType referenceId, DefaultIdType? originalDocumentId, string? reason, string? description, string? notes)
    {
        var mn = memoNumber?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(mn))
            throw new ArgumentException("Memo number is required.");
        if (mn.Length > MaxMemoNumberLength)
            throw new ArgumentException($"Memo number cannot exceed {MaxMemoNumberLength} characters.");

        if (amount <= 0)
            throw new ArgumentException("Debit memo amount must be positive.");

        var rt = referenceType?.Trim() ?? string.Empty;
        if (!IsValidReferenceType(rt))
            throw new ArgumentException($"Invalid reference type: {referenceType}. Must be 'Customer' or 'Vendor'.");

        var rsn = reason?.Trim();
        if (!string.IsNullOrEmpty(rsn) && rsn.Length > MaxReasonLength)
            rsn = rsn.Substring(0, MaxReasonLength);

        MemoNumber = mn;
        MemoDate = memoDate;
        Amount = amount;
        AppliedAmount = 0m;
        ReferenceType = rt;
        ReferenceId = referenceId;
        OriginalDocumentId = originalDocumentId;
        Reason = rsn;
        Status = "Draft";
        ApprovalStatus = "Pending";
        IsApplied = false;
        Description = description?.Trim();
        Notes = notes?.Trim();

        QueueDomainEvent(new DebitMemoCreated(Id, MemoNumber, MemoDate, Amount, ReferenceType, ReferenceId, Reason));
    }

    /// <summary>
    /// Factory method to create a new debit memo with validation.
    /// </summary>
    public static DebitMemo Create(string memoNumber, DateTime memoDate, decimal amount, string referenceType,
        DefaultIdType referenceId, DefaultIdType? originalDocumentId = null, string? reason = null, 
        string? description = null, string? notes = null)
    {
        return new DebitMemo(memoNumber, memoDate, amount, referenceType, referenceId, 
            originalDocumentId, reason, description, notes);
    }

    /// <summary>
    /// Update mutable fields of the debit memo.
    /// Cannot update if already approved or applied.
    /// </summary>
    public DebitMemo Update(DateTime? memoDate = null, decimal? amount = null, string? reason = null,
        string? description = null, string? notes = null)
    {
        if (ApprovalStatus == "Approved")
            throw new InvalidOperationException("Cannot modify approved debit memo.");

        if (IsApplied)
            throw new InvalidOperationException("Cannot modify applied debit memo.");

        bool isUpdated = false;

        if (memoDate.HasValue && MemoDate != memoDate.Value)
        {
            MemoDate = memoDate.Value;
            isUpdated = true;
        }

        if (amount.HasValue && Amount != amount.Value)
        {
            if (amount.Value <= 0)
                throw new ArgumentException("Debit memo amount must be positive.");
            Amount = amount.Value;
            isUpdated = true;
        }

        if (reason != Reason)
        {
            var rsn = reason?.Trim();
            if (!string.IsNullOrEmpty(rsn) && rsn.Length > MaxReasonLength)
                rsn = rsn.Substring(0, MaxReasonLength);
            Reason = rsn;
            isUpdated = true;
        }

        if (description != Description)
        {
            Description = description?.Trim();
            isUpdated = true;
        }

        if (notes != Notes)
        {
            Notes = notes?.Trim();
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new DebitMemoUpdated(this));
        }

        return this;
    }

    /// <summary>
    /// Approve the debit memo for application.
    /// </summary>
    public DebitMemo Approve(string approvedBy)
    {
        if (ApprovalStatus == "Approved")
            throw new InvalidOperationException("Debit memo already approved.");

        if (string.IsNullOrWhiteSpace(approvedBy))
            throw new ArgumentException("ApprovedBy is required.");

        ApprovalStatus = "Approved";
        Status = "Approved";
        ApprovedBy = approvedBy.Trim();
        ApprovedDate = DateTime.UtcNow;

        QueueDomainEvent(new DebitMemoApproved(Id, MemoNumber, ApprovedBy, ApprovedDate.Value));
        return this;
    }

    /// <summary>
    /// Apply the memo (or portion of it) to an invoice or bill.
    /// </summary>
    public DebitMemo Apply(decimal amountToApply, DefaultIdType targetDocumentId)
    {
        if (ApprovalStatus != "Approved")
            throw new InvalidOperationException("Debit memo must be approved before applying.");

        if (amountToApply <= 0)
            throw new ArgumentException("Amount to apply must be positive.");

        if (amountToApply > UnappliedAmount)
            throw new InvalidOperationException("Amount to apply exceeds unapplied balance.");

        AppliedAmount += amountToApply;
        
        if (!IsApplied && AppliedAmount > 0)
        {
            IsApplied = true;
            AppliedDate = DateTime.UtcNow;
            Status = UnappliedAmount == 0 ? "FullyApplied" : "PartiallyApplied";
        }

        QueueDomainEvent(new DebitMemoApplied(Id, MemoNumber, targetDocumentId, amountToApply, UnappliedAmount));
        return this;
    }

    /// <summary>
    /// Void the debit memo.
    /// </summary>
    public DebitMemo Void(string? voidReason = null)
    {
        if (Status == "Voided")
            throw new InvalidOperationException("Debit memo already voided.");

        Status = "Voided";
        QueueDomainEvent(new DebitMemoVoided(Id, MemoNumber, DateTime.UtcNow, voidReason));
        return this;
    }

    private static bool IsValidReferenceType(string referenceType)
    {
        return referenceType == "Customer" || referenceType == "Vendor";
    }
}
