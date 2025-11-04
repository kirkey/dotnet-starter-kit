using Accounting.Domain.Events.Bill;

namespace Accounting.Domain.Entities;

/// <summary>
/// Represents a vendor bill for goods or services received, payable through accounts payable.
/// </summary>
/// <remarks>
/// Use cases:
/// - Record vendor invoices for goods and services purchased.
/// - Track amounts owed to suppliers and vendors.
/// - Support approval workflow before payment processing.
/// - Enable due date tracking and aging analysis.
/// - Integrate with payment processing and check generation.
/// 
/// Default values:
/// - Status: "Draft"
/// - IsPosted: false
/// - IsPaid: false
/// - ApprovalStatus: "Pending"
/// 
/// Business rules:
/// - Bills in "Posted" or "Paid" status cannot be modified.
/// - Bill must be approved before it can be posted.
/// - Total amount must match sum of line items.
/// - Due date must be on or after bill date.
/// - Vendor must be specified and active.
/// </remarks>
/// <seealso cref="Accounting.Domain.Entities.BillLineItem"/>
/// <seealso cref="Accounting.Domain.Events.Bill.BillCreated"/>
/// <seealso cref="Accounting.Domain.Events.Bill.BillUpdated"/>
/// <seealso cref="Accounting.Domain.Events.Bill.BillPosted"/>
/// <seealso cref="Accounting.Domain.Events.Bill.BillApproved"/>
public class Bill : AuditableEntity, IAggregateRoot
{
    private const int MaxBillNumberLength = 50;
    private const int MaxDescriptionLength = 500;
    private const int MaxNotesLength = 2000;

    /// <summary>
    /// Bill number or vendor invoice number for identification.
    /// Example: "INV-2025-001", "BILL-12345". Used for tracking and reconciliation.
    /// </summary>
    public string BillNumber { get; private set; }

    /// <summary>
    /// Vendor identifier who issued this bill.
    /// Reference to the vendor in vendor management system.
    /// </summary>
    public DefaultIdType VendorId { get; private set; }

    /// <summary>
    /// Date when the bill was issued by the vendor.
    /// Example: 2025-11-03. Used for aging and period assignment.
    /// </summary>
    public DateTime BillDate { get; private set; }

    /// <summary>
    /// Date when payment is due to the vendor.
    /// Example: 2025-12-03 (30 days from bill date). Used for payment scheduling.
    /// </summary>
    public DateTime DueDate { get; private set; }

    /// <summary>
    /// Total amount of the bill including all line items.
    /// Automatically calculated from line items. Used for payment processing.
    /// </summary>
    public decimal TotalAmount { get; private set; }

    /// <summary>
    /// Current status of the bill.
    /// Values: "Draft", "Submitted", "Approved", "Posted", "Paid", "Void".
    /// Default: "Draft".
    /// </summary>
    public string Status { get; private set; }

    /// <summary>
    /// Indicates whether the bill has been posted to the general ledger.
    /// Default: false. Once posted, the bill becomes largely immutable.
    /// </summary>
    public bool IsPosted { get; private set; }

    /// <summary>
    /// Indicates whether the bill has been paid in full.
    /// Default: false. Set to true when payment is completed.
    /// </summary>
    public bool IsPaid { get; private set; }

    /// <summary>
    /// Date when the bill was paid.
    /// Set when payment is processed and bill is marked as paid.
    /// </summary>
    public DateTime? PaidDate { get; private set; }

    /// <summary>
    /// Approval status of the bill.
    /// Values: "Pending", "Approved", "Rejected".
    /// Default: "Pending". Must be approved before posting.
    /// </summary>
    public string ApprovalStatus { get; private set; }

    /// <summary>
    /// User identifier who approved or rejected the bill.
    /// Example: "john.doe@company.com". Set during approval workflow.
    /// </summary>
    public string? ApprovedBy { get; private set; }

    /// <summary>
    /// Date/time when the bill was approved or rejected.
    /// Set during approval workflow.
    /// </summary>
    public DateTime? ApprovedDate { get; private set; }

    /// <summary>
    /// Optional accounting period identifier to which this bill belongs.
    /// Links to accounting period for financial reporting.
    /// </summary>
    public DefaultIdType? PeriodId { get; private set; }

    /// <summary>
    /// Optional payment terms description.
    /// Example: "Net 30", "2/10 Net 30", "Due on Receipt".
    /// </summary>
    public string? PaymentTerms { get; private set; }

    /// <summary>
    /// Optional reference to vendor's purchase order.
    /// Example: "PO-2025-123". Used for cross-referencing.
    /// </summary>
    public string? PurchaseOrderNumber { get; private set; }

    // Description and Notes properties are inherited from AuditableEntity base class

    private readonly List<BillLineItem> _lineItems = new();
    /// <summary>
    /// Collection of line items detailing the goods/services on this bill.
    /// Each line item represents a specific charge or expense.
    /// </summary>
    public IReadOnlyCollection<BillLineItem> LineItems => _lineItems.AsReadOnly();

    // EF Core parameterless constructor
    private Bill()
    {
        BillNumber = string.Empty;
        Status = "Draft";
        ApprovalStatus = "Pending";
    }

    // Private constructor with required parameters
    private Bill(
        string billNumber,
        DefaultIdType vendorId,
        DateTime billDate,
        DateTime dueDate,
        string? description = null,
        DefaultIdType? periodId = null,
        string? paymentTerms = null,
        string? purchaseOrderNumber = null,
        string? notes = null)
    {
        if (string.IsNullOrWhiteSpace(billNumber))
            throw new ArgumentException("Bill number is required", nameof(billNumber));

        if (billNumber.Trim().Length > MaxBillNumberLength)
            throw new ArgumentException($"Bill number cannot exceed {MaxBillNumberLength} characters", nameof(billNumber));

        if (vendorId == default)
            throw new ArgumentException("Vendor ID is required", nameof(vendorId));

        if (billDate == default)
            throw new ArgumentException("Bill date is required", nameof(billDate));

        if (dueDate == default)
            throw new ArgumentException("Due date is required", nameof(dueDate));

        if (dueDate < billDate)
            throw new ArgumentException("Due date cannot be before bill date", nameof(dueDate));

        if (!string.IsNullOrWhiteSpace(description) && description.Trim().Length > MaxDescriptionLength)
            throw new ArgumentException($"Description cannot exceed {MaxDescriptionLength} characters", nameof(description));

        if (!string.IsNullOrWhiteSpace(notes) && notes.Trim().Length > MaxNotesLength)
            throw new ArgumentException($"Notes cannot exceed {MaxNotesLength} characters", nameof(notes));

        BillNumber = billNumber.Trim();
        VendorId = vendorId;
        BillDate = billDate.Date;
        DueDate = dueDate.Date;
        Description = description?.Trim();
        PeriodId = periodId;
        PaymentTerms = paymentTerms?.Trim();
        PurchaseOrderNumber = purchaseOrderNumber?.Trim();
        Notes = notes?.Trim();

        TotalAmount = 0;
        Status = "Draft";
        IsPosted = false;
        IsPaid = false;
        ApprovalStatus = "Pending";

        QueueDomainEvent(new BillCreated(Id, BillNumber, VendorId, BillDate, DueDate));
    }

    /// <summary>
    /// Factory method to create a new bill.
    /// </summary>
    /// <param name="billNumber">Unique bill or invoice number.</param>
    /// <param name="vendorId">Vendor who issued the bill.</param>
    /// <param name="billDate">Date bill was issued.</param>
    /// <param name="dueDate">Date payment is due.</param>
    /// <param name="description">Optional description of the bill.</param>
    /// <param name="periodId">Optional accounting period.</param>
    /// <param name="paymentTerms">Optional payment terms.</param>
    /// <param name="purchaseOrderNumber">Optional PO reference.</param>
    /// <param name="notes">Optional notes.</param>
    /// <returns>New Bill instance.</returns>
    public static Bill Create(
        string billNumber,
        DefaultIdType vendorId,
        DateTime billDate,
        DateTime dueDate,
        string? description = null,
        DefaultIdType? periodId = null,
        string? paymentTerms = null,
        string? purchaseOrderNumber = null,
        string? notes = null)
    {
        return new Bill(billNumber, vendorId, billDate, dueDate, description, periodId, paymentTerms, purchaseOrderNumber, notes);
    }

    /// <summary>
    /// Update bill properties.
    /// Throws exception if bill is already posted or paid.
    /// </summary>
    public Bill Update(
        string? billNumber,
        DateTime? billDate,
        DateTime? dueDate,
        string? description,
        DefaultIdType? periodId,
        string? paymentTerms,
        string? purchaseOrderNumber,
        string? notes)
    {
        if (IsPosted)
            throw new BillCannotBeModifiedException(Id, "Bill is already posted");

        if (IsPaid)
            throw new BillCannotBeModifiedException(Id, "Bill is already paid");

        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(billNumber) && BillNumber != billNumber)
        {
            var trimmed = billNumber.Trim();
            if (trimmed.Length > MaxBillNumberLength)
                throw new ArgumentException($"Bill number cannot exceed {MaxBillNumberLength} characters");
            BillNumber = trimmed;
            isUpdated = true;
        }

        if (billDate.HasValue && BillDate != billDate.Value.Date)
        {
            if (billDate.Value > DueDate)
                throw new ArgumentException("Bill date cannot be after due date");
            BillDate = billDate.Value.Date;
            isUpdated = true;
        }

        if (dueDate.HasValue && DueDate != dueDate.Value.Date)
        {
            if (dueDate.Value < BillDate)
                throw new ArgumentException("Due date cannot be before bill date");
            DueDate = dueDate.Value.Date;
            isUpdated = true;
        }

        if (description != Description)
        {
            if (!string.IsNullOrWhiteSpace(description) && description.Trim().Length > MaxDescriptionLength)
                throw new ArgumentException($"Description cannot exceed {MaxDescriptionLength} characters");
            Description = description?.Trim();
            isUpdated = true;
        }

        if (periodId != PeriodId)
        {
            PeriodId = periodId;
            isUpdated = true;
        }

        if (paymentTerms != PaymentTerms)
        {
            PaymentTerms = paymentTerms?.Trim();
            isUpdated = true;
        }

        if (purchaseOrderNumber != PurchaseOrderNumber)
        {
            PurchaseOrderNumber = purchaseOrderNumber?.Trim();
            isUpdated = true;
        }

        if (notes != Notes)
        {
            if (!string.IsNullOrWhiteSpace(notes) && notes.Trim().Length > MaxNotesLength)
                throw new ArgumentException($"Notes cannot exceed {MaxNotesLength} characters");
            Notes = notes?.Trim();
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new BillUpdated(this));
        }

        return this;
    }

    /// <summary>
    /// Update the total amount from line items.
    /// Called by application layer after line item changes.
    /// </summary>
    public Bill UpdateTotalAmount(decimal totalAmount)
    {
        if (totalAmount < 0)
            throw new ArgumentException("Total amount cannot be negative", nameof(totalAmount));

        if (TotalAmount != totalAmount)
        {
            TotalAmount = totalAmount;
        }

        return this;
    }

    /// <summary>
    /// Approve the bill for payment processing.
    /// </summary>
    /// <param name="approvedBy">User who approved the bill.</param>
    public Bill Approve(string approvedBy)
    {
        if (string.IsNullOrWhiteSpace(approvedBy))
            throw new ArgumentException("Approver is required", nameof(approvedBy));

        if (ApprovalStatus == "Approved")
            throw new BillAlreadyApprovedException(Id);

        if (IsPosted)
            throw new BillCannotBeModifiedException(Id, "Bill is already posted");

        ApprovalStatus = "Approved";
        ApprovedBy = approvedBy.Trim();
        ApprovedDate = DateTime.UtcNow;
        Status = "Approved";

        QueueDomainEvent(new BillApproved(Id, approvedBy));
        return this;
    }

    /// <summary>
    /// Reject the bill with reason.
    /// </summary>
    /// <param name="rejectedBy">User who rejected the bill.</param>
    /// <param name="reason">Reason for rejection.</param>
    public Bill Reject(string rejectedBy, string reason)
    {
        if (string.IsNullOrWhiteSpace(rejectedBy))
            throw new ArgumentException("Rejector is required", nameof(rejectedBy));

        if (IsPosted)
            throw new BillCannotBeModifiedException(Id, "Bill is already posted");

        ApprovalStatus = "Rejected";
        ApprovedBy = rejectedBy.Trim();
        ApprovedDate = DateTime.UtcNow;
        Status = "Rejected";
        Notes = string.IsNullOrWhiteSpace(Notes)
            ? $"Rejected: {reason}"
            : $"{Notes}\nRejected: {reason}";

        QueueDomainEvent(new BillRejected(Id, rejectedBy, reason));
        return this;
    }

    /// <summary>
    /// Post the bill to general ledger.
    /// Requires bill to be approved.
    /// </summary>
    public Bill Post()
    {
        if (IsPosted)
            throw new BillAlreadyPostedException(Id);

        if (ApprovalStatus != "Approved")
            throw new BillNotApprovedException(Id);

        if (TotalAmount <= 0)
            throw new BillInvalidAmountException(Id, "Total amount must be greater than zero");

        IsPosted = true;
        Status = "Posted";

        QueueDomainEvent(new BillPosted(Id, BillDate, TotalAmount));
        return this;
    }

    /// <summary>
    /// Mark the bill as paid.
    /// </summary>
    /// <param name="paidDate">Date when payment was made.</param>
    public Bill MarkAsPaid(DateTime paidDate)
    {
        if (!IsPosted)
            throw new BillNotPostedException(Id);

        if (IsPaid)
            throw new BillAlreadyPaidException(Id);

        IsPaid = true;
        PaidDate = paidDate.Date;
        Status = "Paid";

        QueueDomainEvent(new BillPaid(Id, paidDate, TotalAmount));
        return this;
    }

    /// <summary>
    /// Void the bill.
    /// </summary>
    /// <param name="reason">Reason for voiding.</param>
    public Bill Void(string reason)
    {
        if (IsPaid)
            throw new BillCannotBeModifiedException(Id, "Bill is already paid");

        Status = "Void";
        Notes = string.IsNullOrWhiteSpace(Notes)
            ? $"Voided: {reason}"
            : $"{Notes}\nVoided: {reason}";

        QueueDomainEvent(new BillVoided(Id, reason));
        return this;
    }
}

