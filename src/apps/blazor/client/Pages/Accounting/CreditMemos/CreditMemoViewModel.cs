namespace FSH.Starter.Blazor.Client.Pages.Accounting.CreditMemos;

/// <summary>
/// View model for credit memo add/edit operations.
/// Represents a credit memo used to decrease a customer's receivable balance or vendor's payable balance.
/// </summary>
public class CreditMemoViewModel
{
    public DefaultIdType Id { get; set; }
    
    /// <summary>
    /// Unique memo number for tracking and reference.
    /// Example: "CM-2025-001", "CREDIT-OCT-12345"
    /// </summary>
    public string MemoNumber { get; set; } = string.Empty;
    
    /// <summary>
    /// Date the memo was issued.
    /// </summary>
    public DateTime MemoDate { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Amount of the credit adjustment; must be positive.
    /// </summary>
    public decimal Amount { get; set; }
    
    /// <summary>
    /// Amount already applied to invoices/bills.
    /// </summary>
    public decimal AppliedAmount { get; set; }
    
    /// <summary>
    /// Amount refunded directly to customer/vendor.
    /// </summary>
    public decimal RefundedAmount { get; set; }
    
    /// <summary>
    /// Remaining unapplied amount available.
    /// </summary>
    public decimal UnappliedAmount { get; set; }
    
    /// <summary>
    /// Type of reference: "Customer" or "Vendor"
    /// </summary>
    public string ReferenceType { get; set; } = "Customer";
    
    /// <summary>
    /// Identifier of the customer or vendor this memo applies to.
    /// </summary>
    public DefaultIdType ReferenceId { get; set; }
    
    /// <summary>
    /// Optional reference to original invoice or bill being adjusted.
    /// </summary>
    public DefaultIdType? OriginalDocumentId { get; set; }
    
    /// <summary>
    /// Reason for the credit memo.
    /// Example: "Product return - damaged goods"
    /// </summary>
    public string? Reason { get; set; }
    
    /// <summary>
    /// Current status: Draft, Approved, Applied, Refunded, Voided
    /// </summary>
    public string Status { get; set; } = "Draft";
    
    /// <summary>
    /// Whether the memo has been applied to invoices/bills or refunded.
    /// </summary>
    public bool IsApplied { get; set; }
    
    /// <summary>
    /// Date when the memo was applied or refunded.
    /// </summary>
    public DateTime? AppliedDate { get; set; }
    
    /// <summary>
    /// Approval status: Pending, Approved, Rejected
    /// </summary>
    public string ApprovalStatus { get; set; } = "Pending";
    
    /// <summary>
    /// Person who approved the memo.
    /// </summary>
    public string? ApprovedBy { get; set; }
    
    /// <summary>
    /// Date when the memo was approved.
    /// </summary>
    public DateTime? ApprovedDate { get; set; }
    
    /// <summary>
    /// Additional description of the memo.
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// Internal notes about the memo.
    /// </summary>
    public string? Notes { get; set; }
}
