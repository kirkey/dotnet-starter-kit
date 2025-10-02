using Accounting.Domain.Events.CreditMemo;
using Accounting.Domain.Constants;

namespace Accounting.Domain.Entities;

/// <summary>
/// Represents a credit memo used to decrease a customer's receivable balance or vendor's payable balance.
/// </summary>
/// <remarks>
/// Use cases:
/// - Issue credits to customers for returns, refunds, or overbilling corrections.
/// - Reduce vendor payables for returns, rebates, or billing errors.
/// - Document price reductions, discounts, or promotional credits.
/// - Handle service failures, quality issues, or customer satisfaction adjustments.
/// - Support billing corrections with proper audit trail and approval workflow.
/// - Enable integration with accounts receivable and payable management.
/// 
/// Default values:
/// - MemoNumber: required unique identifier (example: "CM-2025-001")
/// - MemoDate: required date when memo is issued (example: 2025-10-02)
/// - Amount: required positive amount (example: 250.00)
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
/// - Can be applied to multiple invoices/bills or issued as refund
/// - Must have proper authorization before approval
/// - Voiding reverses any applications
/// - Credits reduce receivables (customer) or payables (vendor)
/// </remarks>
/// <seealso cref="Accounting.Domain.Events.CreditMemo.CreditMemoCreated"/>
/// <seealso cref="Accounting.Domain.Events.CreditMemo.CreditMemoUpdated"/>
/// <seealso cref="Accounting.Domain.Events.CreditMemo.CreditMemoApproved"/>
/// <seealso cref="Accounting.Domain.Events.CreditMemo.CreditMemoApplied"/>
/// <seealso cref="Accounting.Domain.Events.CreditMemo.CreditMemoRefunded"/>
/// <seealso cref="Accounting.Domain.Events.CreditMemo.CreditMemoVoided"/>
public class CreditMemo : AuditableEntity, IAggregateRoot
{
    private const int MaxMemoNumberLength = 50;
    private const int MaxReferenceTypeLength = 20;
    private const int MaxReasonLength = 500;

    /// <summary>
    /// Unique memo number for tracking and reference.
    /// Example: "CM-2025-001", "CREDIT-OCT-12345"
    /// </summary>
    public string MemoNumber { get; private set; }

    /// <summary>
    /// Date the memo was issued.
    /// </summary>
    public DateTime MemoDate { get; private set; }

    /// <summary>
    /// Amount of the credit adjustment; must be positive.
    /// Example: 250.00 for returned goods credit
    /// </summary>
    public decimal Amount { get; private set; }

    /// <summary>
    /// Amount already applied to invoices/bills.
    /// Default: 0.00
    /// </summary>
    public decimal AppliedAmount { get; private set; }

    /// <summary>
    /// Amount refunded directly to customer/vendor.
    /// Default: 0.00
    /// </summary>
    public decimal RefundedAmount { get; private set; }

    /// <summary>
    /// Remaining unapplied amount available.
    /// Calculated as Amount - AppliedAmount - RefundedAmount
    /// </summary>
    public decimal UnappliedAmount => Amount - AppliedAmount - RefundedAmount;

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
    /// Reason for the credit memo.
    /// Example: "Product return - damaged goods", "Overbilling correction per audit", "Customer satisfaction adjustment"
    /// </summary>
    public string? Reason { get; private set; }

    /// <summary>
    /// Current status: Draft, Approved, Applied, Refunded, Voided
    /// </summary>
    public string Status { get; private set; }

    /// <summary>
    /// Whether the memo has been applied to invoices/bills or refunded.
    /// </summary>
    public bool IsApplied { get; private set; }

    /// <summary>
    /// Date when the memo was applied or refunded.
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

    private CreditMemo()
    {
        MemoNumber = string.Empty;
        ReferenceType = string.Empty;
        Status = "Draft";
        ApprovalStatus = "Pending";
    }

    private CreditMemo(string memoNumber, DateTime memoDate, decimal amount, string referenceType,
        DefaultIdType referenceId, DefaultIdType? originalDocumentId, string? reason, string? description, string? notes)
    {
        var mn = memoNumber?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(mn))
            throw new ArgumentException("Memo number is required.");
        if (mn.Length > MaxMemoNumberLength)
            throw new ArgumentException($"Memo number cannot exceed {MaxMemoNumberLength} characters.");

        if (amount <= 0)
            throw new ArgumentException("Credit memo amount must be positive.");

        var rt = referenceType?.Trim() ?? string.Empty;
        if (!IsValidReferenceType(rt))
            throw new Accounting.Domain.Exceptions.InvalidCreditMemoReferenceTypeException(referenceType);

        var rsn = reason?.Trim();
        if (!string.IsNullOrEmpty(rsn) && rsn.Length > MaxReasonLength)
            rsn = rsn.Substring(0, MaxReasonLength);

        MemoNumber = mn;
        MemoDate = memoDate;
        Amount = amount;
        AppliedAmount = 0m;
        RefundedAmount = 0m;
        ReferenceType = rt;
        ReferenceId = referenceId;
        OriginalDocumentId = originalDocumentId;
        Reason = rsn;
        Status = "Draft";
        ApprovalStatus = "Pending";
        IsApplied = false;
        Description = description?.Trim();
        Notes = notes?.Trim();

        QueueDomainEvent(new CreditMemoCreated(Id, MemoNumber, MemoDate, Amount, ReferenceType, ReferenceId, Reason));
    }

    /// <summary>
    /// Factory method to create a new credit memo with validation.
    /// </summary>
    public static CreditMemo Create(string memoNumber, DateTime memoDate, decimal amount, string referenceType,
        DefaultIdType referenceId, DefaultIdType? originalDocumentId = null, string? reason = null,
        string? description = null, string? notes = null)
    {
        return new CreditMemo(memoNumber, memoDate, amount, referenceType, referenceId,
            originalDocumentId, reason, description, notes);
    }

    /// <summary>
    /// Update mutable fields of the credit memo.
    /// Cannot update if already approved or applied.
    /// </summary>
    public CreditMemo Update(DateTime? memoDate = null, decimal? amount = null, string? reason = null,
        string? description = null, string? notes = null)
    {
        if (ApprovalStatus == "Approved")
            throw new InvalidOperationException("Cannot modify approved credit memo.");

        if (IsApplied)
            throw new InvalidOperationException("Cannot modify applied credit memo.");

        bool isUpdated = false;

        if (memoDate.HasValue && MemoDate != memoDate.Value)
        {
            MemoDate = memoDate.Value;
            isUpdated = true;
        }

        if (amount.HasValue && Amount != amount.Value)
        {
            if (amount.Value <= 0)
                throw new ArgumentException("Credit memo amount must be positive.");
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
            QueueDomainEvent(new CreditMemoUpdated(this));
        }

        return this;
    }

    /// <summary>
    /// Approve the credit memo for application or refund.
    /// </summary>
    public CreditMemo Approve(string approvedBy)
    {
        if (ApprovalStatus == "Approved")
            throw new InvalidOperationException("Credit memo already approved.");

        if (string.IsNullOrWhiteSpace(approvedBy))
            throw new ArgumentException("ApprovedBy is required.");

        ApprovalStatus = "Approved";
        Status = "Approved";
        ApprovedBy = approvedBy.Trim();
        ApprovedDate = DateTime.UtcNow;

        QueueDomainEvent(new CreditMemoApproved(Id, MemoNumber, ApprovedBy, ApprovedDate.Value));
        return this;
    }

    /// <summary>
    /// Apply the memo (or portion of it) to an invoice or bill.
    /// </summary>
    public CreditMemo Apply(decimal amountToApply, DefaultIdType targetDocumentId)
    {
        if (ApprovalStatus != "Approved")
            throw new InvalidOperationException("Credit memo must be approved before applying.");

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

        QueueDomainEvent(new CreditMemoApplied(Id, MemoNumber, targetDocumentId, amountToApply, UnappliedAmount));
        return this;
    }

    /// <summary>
    /// Issue a refund for the credit memo amount (or portion).
    /// </summary>
    public CreditMemo Refund(decimal amountToRefund, string? refundMethod = null, string? refundReference = null)
    {
        if (ApprovalStatus != "Approved")
            throw new InvalidOperationException("Credit memo must be approved before refunding.");

        if (amountToRefund <= 0)
            throw new ArgumentException("Refund amount must be positive.");

        if (amountToRefund > UnappliedAmount)
            throw new InvalidOperationException("Refund amount exceeds unapplied balance.");

        RefundedAmount += amountToRefund;

        if (!IsApplied && RefundedAmount > 0)
        {
            IsApplied = true;
            AppliedDate = DateTime.UtcNow;
            Status = UnappliedAmount == 0 ? "FullyRefunded" : "PartiallyRefunded";
        }

        QueueDomainEvent(new CreditMemoRefunded(Id, MemoNumber, amountToRefund, DateTime.UtcNow, refundMethod, refundReference));
        return this;
    }

    /// <summary>
    /// Void the credit memo.
    /// </summary>
    public CreditMemo Void(string? voidReason = null)
    {
        if (Status == "Voided")
            throw new InvalidOperationException("Credit memo already voided.");

        Status = "Voided";
        QueueDomainEvent(new CreditMemoVoided(Id, MemoNumber, DateTime.UtcNow, voidReason));
        return this;
    }

    private static bool IsValidReferenceType(string referenceType)
    {
        return MemoReferenceTypes.Contains(referenceType);
    }
}
