using Accounting.Domain.Events.Bill;

namespace Accounting.Domain.Entities;

/// <summary>
/// Represents a vendor bill/invoice for accounts payable processing with approval workflow, payment tracking, and 3-way matching support.
/// </summary>
/// <remarks>
/// Use cases:
/// - Track vendor bills and invoices for accounts payable processing.
/// - Support approval workflow with multi-level authorization.
/// - Enable 3-way matching (purchase order, receipt, invoice).
/// - Track payment application and outstanding balances.
/// - Manage early payment discounts and terms.
/// - Support line-item detail with account coding.
/// - Enable aging analysis for AP management.
/// - Track bill status through approval and payment lifecycle.
/// 
/// Default values:
/// - BillNumber: required unique identifier (example: "BILL-2025-001234")
/// - VendorInvoiceNumber: required vendor's invoice number
/// - Status: "Draft" (new bills start as draft)
/// - BillDate: required date of bill
/// - DueDate: required payment due date
/// - SubtotalAmount: required line items subtotal
/// - TaxAmount: 0.00 (sales tax if applicable)
/// - ShippingAmount: 0.00 (shipping charges if applicable)
/// - DiscountAmount: 0.00 (early payment discount available)
/// - TotalAmount: calculated (Subtotal + Tax + Shipping)
/// - PaidAmount: 0.00 (no payment initially)
/// - IsApproved: false (requires approval)
/// - IsVoid: false (active by default)
/// 
/// Business rules:
/// - BillNumber must be unique within the system
/// - TotalAmount = SubtotalAmount + TaxAmount + ShippingAmount
/// - OutstandingAmount = TotalAmount - PaidAmount
/// - Cannot modify approved bills without reverting approval
/// - Cannot void bills with payments applied
/// - Payment application updates PaidAmount and Status
/// - Discount only available if paid by DiscountDate
/// - 3-way matching validates PO, receipt, and invoice amounts
/// - Status transitions: Draft → PendingApproval → Approved → Paid/Void
/// </remarks>
/// <seealso cref="Accounting.Domain.Events.Bill.BillCreated"/>
/// <seealso cref="Accounting.Domain.Events.Bill.BillUpdated"/>
/// <seealso cref="Accounting.Domain.Events.Bill.BillSubmittedForApproval"/>
/// <seealso cref="Accounting.Domain.Events.Bill.BillApproved"/>
/// <seealso cref="Accounting.Domain.Events.Bill.BillRejected"/>
/// <seealso cref="Accounting.Domain.Events.Bill.BillPaymentApplied"/>
/// <seealso cref="Accounting.Domain.Events.Bill.BillVoided"/>
public class Bill : AuditableEntity, IAggregateRoot
{
    private const int MaxBillNumberLength = 50;
    private const int MaxVendorInvoiceNumberLength = 100;
    private const int MaxStatusLength = 32;
    private const int MaxPaymentTermsLength = 100;
    private const int MaxPaymentMethodLength = 50;
    private const int MaxPaymentReferenceLength = 50;
    private const int MaxApprovedByLength = 256;
    private const int MaxRejectionReasonLength = 1000;
    private const int MaxDescriptionLength = 2048;
    private const int MaxNotesLength = 2048;

    /// <summary>
    /// Unique bill number assigned by the system.
    /// Example: "BILL-2025-001234", "AP-10001". Max length: 50.
    /// </summary>
    public string BillNumber { get; private set; } = string.Empty;

    /// <summary>
    /// Reference to the vendor who issued this bill.
    /// Links to Vendor entity. Required for all bills.
    /// </summary>
    public DefaultIdType VendorId { get; private set; }

    /// <summary>
    /// Vendor's invoice number from their system.
    /// Example: "INV-98765", "2025-001". Max length: 100.
    /// Required for matching and reference.
    /// </summary>
    public string VendorInvoiceNumber { get; private set; } = string.Empty;

    /// <summary>
    /// Date the bill was issued by the vendor.
    /// Example: 2025-10-15. Used for aging and payment scheduling.
    /// </summary>
    public DateTime BillDate { get; private set; }

    /// <summary>
    /// Date the bill payment is due.
    /// Example: 2025-11-14 for Net 30 terms. Used for payment scheduling.
    /// </summary>
    public DateTime DueDate { get; private set; }

    /// <summary>
    /// Subtotal amount before tax and shipping.
    /// Example: 5000.00. Sum of all line item amounts.
    /// </summary>
    public decimal SubtotalAmount { get; private set; }

    /// <summary>
    /// Sales tax amount.
    /// Example: 300.00. Default: 0.00 if no tax.
    /// </summary>
    public decimal TaxAmount { get; private set; }

    /// <summary>
    /// Shipping and handling charges.
    /// Example: 100.00. Default: 0.00 if no shipping.
    /// </summary>
    public decimal ShippingAmount { get; private set; }

    /// <summary>
    /// Early payment discount amount available.
    /// Example: 100.00 for 2% discount. Default: 0.00 if no discount.
    /// </summary>
    public decimal DiscountAmount { get; private set; }

    /// <summary>
    /// Date by which payment must be made to receive discount.
    /// Example: 2025-10-25 for 2/10 Net 30 terms. Null if no discount.
    /// </summary>
    public DateTime? DiscountDate { get; private set; }

    /// <summary>
    /// Total amount due including all charges.
    /// Calculated: SubtotalAmount + TaxAmount + ShippingAmount.
    /// Example: 5400.00.
    /// </summary>
    public decimal TotalAmount { get; private set; }

    /// <summary>
    /// Amount paid to date.
    /// Example: 2700.00 for partial payment. Default: 0.00.
    /// Updated with each payment application.
    /// </summary>
    public decimal PaidAmount { get; private set; }

    /// <summary>
    /// Payment terms description.
    /// Example: "Net 30", "2/10 Net 30", "Due on Receipt". Max length: 100.
    /// </summary>
    public string? PaymentTerms { get; private set; }

    /// <summary>
    /// Current status of the bill.
    /// Values: "Draft", "PendingApproval", "Approved", "Rejected", "Paid", "PartiallyPaid", "Void".
    /// Default: "Draft". Max length: 32.
    /// </summary>
    public string Status { get; private set; } = string.Empty;

    /// <summary>
    /// Whether the bill has been approved for payment.
    /// Default: false. True after approval process completes.
    /// </summary>
    public bool IsApproved { get; private set; }

    /// <summary>
    /// Date when the bill was approved.
    /// Example: 2025-10-20. Null if not yet approved.
    /// </summary>
    public DateTime? ApprovalDate { get; private set; }

    /// <summary>
    /// Person who approved the bill.
    /// Example: "john.doe@company.com", "Manager Name". Max length: 256.
    /// </summary>
    public string? ApprovedBy { get; private set; }

    /// <summary>
    /// Whether the bill has been voided/cancelled.
    /// Default: false. True if bill is cancelled.
    /// </summary>
    public bool IsVoid { get; private set; }

    /// <summary>
    /// Date when the bill was paid in full.
    /// Example: 2025-11-10. Null if not yet paid.
    /// </summary>
    public DateTime? PaidDate { get; private set; }

    /// <summary>
    /// Payment method used.
    /// Example: "Check", "ACH", "Wire", "Credit Card". Max length: 50.
    /// </summary>
    public string? PaymentMethod { get; private set; }

    /// <summary>
    /// Payment reference number.
    /// Example: "CHK-12345", "ACH-98765". Max length: 50.
    /// </summary>
    public string? PaymentReference { get; private set; }

    /// <summary>
    /// Optional purchase order reference for 3-way matching.
    /// Links to PurchaseOrder entity if applicable.
    /// </summary>
    public DefaultIdType? PurchaseOrderId { get; private set; }

    /// <summary>
    /// Reason for rejection if bill was rejected.
    /// Example: "Incorrect amount", "Missing documentation". Max length: 1000.
    /// </summary>
    public string? RejectionReason { get; private set; }

    /// <summary>
    /// Optional GL account for expense coding.
    /// Links to ChartOfAccount entity for default account.
    /// </summary>
    public DefaultIdType? ExpenseAccountId { get; private set; }

    /// <summary>
    /// Optional accounting period for reporting.
    /// Links to AccountingPeriod entity for period tracking.
    /// </summary>
    public DefaultIdType? PeriodId { get; private set; }

    private readonly List<BillLineItem> _lineItems = new();
    /// <summary>
    /// Collection of line items for this bill.
    /// Each line item contains description, quantity, price, and total.
    /// </summary>
    public IReadOnlyCollection<BillLineItem> LineItems => _lineItems.AsReadOnly();

    // Parameterless constructor for EF Core
    private Bill()
    {
        BillNumber = string.Empty;
        VendorInvoiceNumber = string.Empty;
        Status = "Draft";
    }

    private Bill(string billNumber, DefaultIdType vendorId, string vendorInvoiceNumber,
        DateTime billDate, DateTime dueDate, decimal subtotalAmount, decimal taxAmount,
        decimal shippingAmount, string? paymentTerms = null, decimal discountAmount = 0,
        DateTime? discountDate = null, DefaultIdType? purchaseOrderId = null,
        DefaultIdType? expenseAccountId = null, DefaultIdType? periodId = null,
        string? description = null, string? notes = null)
    {
        // Validations
        if (string.IsNullOrWhiteSpace(billNumber))
            throw new ArgumentException("Bill number is required", nameof(billNumber));

        if (billNumber.Length > MaxBillNumberLength)
            throw new ArgumentException($"Bill number cannot exceed {MaxBillNumberLength} characters", nameof(billNumber));

        if (string.IsNullOrWhiteSpace(vendorInvoiceNumber))
            throw new ArgumentException("Vendor invoice number is required", nameof(vendorInvoiceNumber));

        if (vendorInvoiceNumber.Length > MaxVendorInvoiceNumberLength)
            throw new ArgumentException($"Vendor invoice number cannot exceed {MaxVendorInvoiceNumberLength} characters", nameof(vendorInvoiceNumber));

        if (dueDate < billDate)
            throw new ArgumentException("Due date must be on or after bill date", nameof(dueDate));

        if (subtotalAmount < 0)
            throw new ArgumentException("Subtotal amount cannot be negative", nameof(subtotalAmount));

        if (taxAmount < 0)
            throw new ArgumentException("Tax amount cannot be negative", nameof(taxAmount));

        if (shippingAmount < 0)
            throw new ArgumentException("Shipping amount cannot be negative", nameof(shippingAmount));

        if (discountAmount < 0)
            throw new ArgumentException("Discount amount cannot be negative", nameof(discountAmount));

        BillNumber = billNumber.Trim();
        Name = billNumber.Trim(); // For AuditableEntity compatibility
        VendorId = vendorId;
        VendorInvoiceNumber = vendorInvoiceNumber.Trim();
        BillDate = billDate;
        DueDate = dueDate;
        SubtotalAmount = subtotalAmount;
        TaxAmount = taxAmount;
        ShippingAmount = shippingAmount;
        DiscountAmount = discountAmount;
        DiscountDate = discountDate;
        TotalAmount = subtotalAmount + taxAmount + shippingAmount;
        PaidAmount = 0m;
        PaymentTerms = paymentTerms?.Trim();
        Status = "Draft";
        IsApproved = false;
        IsVoid = false;
        PurchaseOrderId = purchaseOrderId;
        ExpenseAccountId = expenseAccountId;
        PeriodId = periodId;
        Description = description?.Trim();
        Notes = notes?.Trim();

        QueueDomainEvent(new BillCreated(Id, BillNumber, VendorId, VendorInvoiceNumber, BillDate, TotalAmount, Description, Notes));
    }

    /// <summary>
    /// Factory method to create a new bill with validation.
    /// </summary>
    public static Bill Create(string billNumber, DefaultIdType vendorId, string vendorInvoiceNumber,
        DateTime billDate, DateTime dueDate, decimal subtotalAmount, decimal taxAmount,
        decimal shippingAmount, string? paymentTerms = null, decimal discountAmount = 0,
        DateTime? discountDate = null, DefaultIdType? purchaseOrderId = null,
        DefaultIdType? expenseAccountId = null, DefaultIdType? periodId = null,
        string? description = null, string? notes = null)
    {
        return new Bill(billNumber, vendorId, vendorInvoiceNumber, billDate, dueDate,
            subtotalAmount, taxAmount, shippingAmount, paymentTerms, discountAmount,
            discountDate, purchaseOrderId, expenseAccountId, periodId, description, notes);
    }

    /// <summary>
    /// Update bill details; not allowed when approved or paid.
    /// </summary>
    public Bill Update(DateTime? dueDate = null, decimal? subtotalAmount = null,
        decimal? taxAmount = null, decimal? shippingAmount = null,
        string? paymentTerms = null, string? description = null, string? notes = null)
    {
        if (IsApproved)
            throw new InvalidOperationException("Cannot modify approved bill");

        if (Status == "Paid")
            throw new InvalidOperationException("Cannot modify paid bill");

        bool isUpdated = false;

        if (dueDate.HasValue && DueDate != dueDate.Value)
        {
            if (dueDate.Value < BillDate)
                throw new ArgumentException("Due date must be on or after bill date");
            DueDate = dueDate.Value;
            isUpdated = true;
        }

        if (subtotalAmount.HasValue && SubtotalAmount != subtotalAmount.Value)
        {
            if (subtotalAmount.Value < 0)
                throw new ArgumentException("Subtotal amount cannot be negative");
            SubtotalAmount = subtotalAmount.Value;
            RecalculateTotalAmount();
            isUpdated = true;
        }

        if (taxAmount.HasValue && TaxAmount != taxAmount.Value)
        {
            if (taxAmount.Value < 0)
                throw new ArgumentException("Tax amount cannot be negative");
            TaxAmount = taxAmount.Value;
            RecalculateTotalAmount();
            isUpdated = true;
        }

        if (shippingAmount.HasValue && ShippingAmount != shippingAmount.Value)
        {
            if (shippingAmount.Value < 0)
                throw new ArgumentException("Shipping amount cannot be negative");
            ShippingAmount = shippingAmount.Value;
            RecalculateTotalAmount();
            isUpdated = true;
        }

        if (paymentTerms != PaymentTerms)
        {
            PaymentTerms = paymentTerms?.Trim();
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
            QueueDomainEvent(new BillUpdated(Id, BillNumber, TotalAmount, Description, Notes));
        }

        return this;
    }

    /// <summary>
    /// Submit bill for approval.
    /// </summary>
    public Bill SubmitForApproval()
    {
        if (Status != "Draft")
            throw new InvalidOperationException("Can only submit draft bills for approval");

        if (_lineItems.Count == 0)
            throw new InvalidOperationException("Cannot submit bill with no line items");

        Status = "PendingApproval";

        QueueDomainEvent(new BillSubmittedForApproval(Id, BillNumber, VendorId, TotalAmount));
        return this;
    }

    /// <summary>
    /// Approve the bill for payment.
    /// </summary>
    public Bill Approve(string approvedBy)
    {
        if (string.IsNullOrWhiteSpace(approvedBy))
            throw new ArgumentException("Approver information is required", nameof(approvedBy));

        if (Status != "PendingApproval")
            throw new InvalidOperationException("Can only approve bills pending approval");

        IsApproved = true;
        ApprovalDate = DateTime.UtcNow;
        ApprovedBy = approvedBy.Trim();
        Status = "Approved";

        QueueDomainEvent(new BillApproved(Id, BillNumber, ApprovedBy, ApprovalDate.Value, TotalAmount));
        return this;
    }

    /// <summary>
    /// Reject the bill with reason.
    /// </summary>
    public Bill Reject(string rejectionReason)
    {
        if (string.IsNullOrWhiteSpace(rejectionReason))
            throw new ArgumentException("Rejection reason is required", nameof(rejectionReason));

        if (Status != "PendingApproval")
            throw new InvalidOperationException("Can only reject bills pending approval");

        Status = "Rejected";
        RejectionReason = rejectionReason.Trim();

        QueueDomainEvent(new BillRejected(Id, BillNumber, RejectionReason));
        return this;
    }

    /// <summary>
    /// Revert approval to allow modifications.
    /// </summary>
    public Bill RevertApproval()
    {
        if (!IsApproved)
            throw new InvalidOperationException("Bill is not approved");

        if (PaidAmount > 0)
            throw new InvalidOperationException("Cannot revert approval on bill with payments");

        IsApproved = false;
        ApprovalDate = null;
        ApprovedBy = null;
        Status = "Draft";

        return this;
    }

    /// <summary>
    /// Apply a payment to the bill.
    /// </summary>
    public Bill ApplyPayment(decimal amount, DateTime paymentDate, string? paymentMethod = null, string? paymentReference = null)
    {
        if (!IsApproved)
            throw new InvalidOperationException("Can only apply payment to approved bills");

        if (amount <= 0)
            throw new ArgumentException("Payment amount must be positive", nameof(amount));

        if (amount > OutstandingAmount)
            throw new ArgumentException($"Payment amount {amount:N2} exceeds outstanding balance {OutstandingAmount:N2}");

        PaidAmount += amount;
        Status = PaidAmount >= TotalAmount ? "Paid" : "PartiallyPaid";

        if (Status == "Paid")
        {
            PaidDate = paymentDate;
            PaymentMethod = paymentMethod?.Trim();
            PaymentReference = paymentReference?.Trim();
        }

        QueueDomainEvent(new BillPaymentApplied(Id, BillNumber, amount, PaidAmount, OutstandingAmount, paymentDate));
        return this;
    }

    /// <summary>
    /// Void the bill.
    /// </summary>
    public Bill Void(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Void reason is required", nameof(reason));

        if (PaidAmount > 0)
            throw new InvalidOperationException("Cannot void bill with payments applied");

        IsVoid = true;
        Status = "Void";
        Notes = $"{Notes}\n\nVoided: {reason.Trim()}".Trim();

        QueueDomainEvent(new BillVoided(Id, BillNumber, reason));
        return this;
    }

    /// <summary>
    /// Add a line item to the bill.
    /// </summary>
    public Bill AddLineItem(string description, decimal quantity, decimal unitPrice, DefaultIdType? accountId = null)
    {
        if (IsApproved)
            throw new InvalidOperationException("Cannot add line items to approved bill");

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Line item description is required", nameof(description));

        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(quantity));

        if (unitPrice < 0)
            throw new ArgumentException("Unit price cannot be negative", nameof(unitPrice));

        var lineItem = BillLineItem.Create(description, quantity, unitPrice, accountId);
        _lineItems.Add(lineItem);

        // Recalculate subtotal
        SubtotalAmount = _lineItems.Sum(li => li.LineTotal);
        RecalculateTotalAmount();

        return this;
    }

    /// <summary>
    /// Remove a line item from the bill.
    /// </summary>
    public Bill RemoveLineItem(BillLineItem lineItem)
    {
        if (IsApproved)
            throw new InvalidOperationException("Cannot remove line items from approved bill");

        _lineItems.Remove(lineItem);

        // Recalculate subtotal
        SubtotalAmount = _lineItems.Sum(li => li.LineTotal);
        RecalculateTotalAmount();

        return this;
    }

    private void RecalculateTotalAmount()
    {
        TotalAmount = SubtotalAmount + TaxAmount + ShippingAmount;
    }

    /// <summary>
    /// Outstanding amount remaining to be paid.
    /// </summary>
    public decimal OutstandingAmount => TotalAmount - PaidAmount;

    /// <summary>
    /// Whether the bill is overdue for payment.
    /// </summary>
    public bool IsOverdue => Status != "Paid" && Status != "Void" && DateTime.UtcNow.Date > DueDate.Date;

    /// <summary>
    /// Number of days past the due date.
    /// </summary>
    public int DaysPastDue => IsOverdue ? (DateTime.UtcNow.Date - DueDate.Date).Days : 0;

    /// <summary>
    /// Whether early payment discount is still available.
    /// </summary>
    public bool IsDiscountAvailable => DiscountDate.HasValue && DateTime.UtcNow.Date <= DiscountDate.Value.Date && PaidAmount == 0;
}

/// <summary>
/// Represents a line item on a bill.
/// </summary>
public class BillLineItem
{
    /// <summary>
    /// Description of the item or service.
    /// Example: "Office supplies", "Consulting services". Max length: 500.
    /// </summary>
    public string Description { get; private set; } = string.Empty;

    /// <summary>
    /// Quantity of items.
    /// Example: 10.0 for 10 units, 2.5 for 2.5 hours. Must be positive.
    /// </summary>
    public decimal Quantity { get; private set; }

    /// <summary>
    /// Price per unit.
    /// Example: 50.00 for $50 per unit. Cannot be negative.
    /// </summary>
    public decimal UnitPrice { get; private set; }

    /// <summary>
    /// Total line amount (Quantity × UnitPrice).
    /// Example: 500.00 for 10 units at $50 each.
    /// </summary>
    public decimal LineTotal { get; private set; }

    /// <summary>
    /// Optional GL account for expense coding.
    /// Links to ChartOfAccount entity if specified.
    /// </summary>
    public DefaultIdType? AccountId { get; private set; }

    private BillLineItem()
    {
        // EF Core constructor
    }

    private BillLineItem(string description, decimal quantity, decimal unitPrice, DefaultIdType? accountId)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description is required", nameof(description));

        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive", nameof(quantity));

        if (unitPrice < 0)
            throw new ArgumentException("Unit price cannot be negative", nameof(unitPrice));

        Description = description.Trim();
        Quantity = quantity;
        UnitPrice = unitPrice;
        LineTotal = quantity * unitPrice;
        AccountId = accountId;
    }

    public static BillLineItem Create(string description, decimal quantity, decimal unitPrice, DefaultIdType? accountId = null)
    {
        return new BillLineItem(description, quantity, unitPrice, accountId);
    }
}

