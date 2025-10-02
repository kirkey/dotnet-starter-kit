using Accounting.Domain.Events.PurchaseOrder;

namespace Accounting.Domain;

/// <summary>
/// Represents a purchase order for formal procurement tracking and three-way matching with receipts and invoices.
/// </summary>
/// <remarks>
/// Use cases:
/// - Formalize procurement requests and approvals before ordering.
/// - Track expected deliveries and match with receipts and bills.
/// - Enable three-way matching (PO, Receipt, Invoice) for payment control.
/// - Support budgetary control and commitment accounting.
/// - Maintain audit trail of procurement activities and approvals.
/// - Facilitate vendor management and delivery tracking.
/// - Enable accrual of goods/services received but not yet invoiced.
/// 
/// Default values:
/// - Status: Draft (new POs start as draft)
/// - OrderDate: current date (when PO is created)
/// - TotalAmount: 0.00 (calculated from line items)
/// - ReceivedAmount: 0.00 (updated as items are received)
/// - BilledAmount: 0.00 (updated as invoices are matched)
/// - ApprovalStatus: Pending (requires approval before sending)
/// - IsFullyReceived: false (becomes true when all items received)
/// - IsFullyBilled: false (becomes true when fully invoiced)
/// 
/// Business rules:
/// - Must have at least one line item with valid account and amount
/// - Cannot modify PO once approved (except for closure)
/// - Total amount must match sum of line items
/// - Received quantity cannot exceed ordered quantity
/// - Billed amount cannot exceed received amount (typically)
/// - Requires approval before sending to vendor
/// - Can close PO even if not fully received (short shipments)
/// - Three-way matching: PO → Receipt → Invoice validation
/// </remarks>
public class PurchaseOrder : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Unique purchase order number.
    /// </summary>
    public string OrderNumber { get; private set; } = string.Empty;

    /// <summary>
    /// Date when purchase order was created.
    /// </summary>
    public DateTime OrderDate { get; private set; }

    /// <summary>
    /// Expected delivery date.
    /// </summary>
    public DateTime? ExpectedDeliveryDate { get; private set; }

    /// <summary>
    /// Vendor/supplier receiving the purchase order.
    /// </summary>
    public DefaultIdType VendorId { get; private set; }

    /// <summary>
    /// Vendor name for display purposes.
    /// </summary>
    public string VendorName { get; private set; } = string.Empty;

    /// <summary>
    /// Total amount of the purchase order.
    /// </summary>
    public decimal TotalAmount { get; private set; }

    /// <summary>
    /// Amount of goods/services received (based on receipts).
    /// </summary>
    public decimal ReceivedAmount { get; private set; }

    /// <summary>
    /// Amount invoiced by vendor (based on matched bills).
    /// </summary>
    public decimal BilledAmount { get; private set; }

    /// <summary>
    /// Purchase order status: Draft, Approved, Sent, PartiallyReceived, Received, Closed, Cancelled.
    /// </summary>
    public PurchaseOrderStatus Status { get; private set; }

    /// <summary>
    /// Approval status: Pending, Approved, Rejected.
    /// </summary>
    public ApprovalStatus ApprovalStatus { get; private set; }

    /// <summary>
    /// Whether all items have been received.
    /// </summary>
    public bool IsFullyReceived { get; private set; }

    /// <summary>
    /// Whether all items have been billed.
    /// </summary>
    public bool IsFullyBilled { get; private set; }

    /// <summary>
    /// User who approved the purchase order.
    /// </summary>
    public string? ApprovedBy { get; private set; }

    /// <summary>
    /// Date when purchase order was approved.
    /// </summary>
    public DateTime? ApprovedDate { get; private set; }

    /// <summary>
    /// Optional requester/originator of the purchase order.
    /// </summary>
    public DefaultIdType? RequesterId { get; private set; }

    /// <summary>
    /// Requester name for display purposes.
    /// </summary>
    public string? RequesterName { get; private set; }

    /// <summary>
    /// Optional cost center for expense allocation.
    /// </summary>
    public DefaultIdType? CostCenterId { get; private set; }

    /// <summary>
    /// Optional project for job costing.
    /// </summary>
    public DefaultIdType? ProjectId { get; private set; }

    /// <summary>
    /// Shipping address for delivery.
    /// </summary>
    public string? ShipToAddress { get; private set; }

    /// <summary>
    /// Payment terms (e.g., "Net 30", "2/10 Net 30").
    /// </summary>
    public string? PaymentTerms { get; private set; }

    /// <summary>
    /// Optional reference to related requisition or request.
    /// </summary>
    public string? ReferenceNumber { get; private set; }

    /// <summary>
    /// Optional description or purpose of the purchase.
    /// </summary>
    public new string? Description { get; private set; }

    /// <summary>
    /// Optional notes or special instructions.
    /// </summary>
    public new string? Notes { get; private set; }

    // Parameterless constructor for EF Core
    private PurchaseOrder()
    {
    }

    private PurchaseOrder(
        string orderNumber,
        DateTime orderDate,
        DefaultIdType vendorId,
        string vendorName,
        DefaultIdType? requesterId = null,
        string? requesterName = null,
        DefaultIdType? costCenterId = null,
        DefaultIdType? projectId = null,
        DateTime? expectedDeliveryDate = null,
        string? shipToAddress = null,
        string? paymentTerms = null,
        string? referenceNumber = null,
        string? description = null,
        string? notes = null)
    {
        if (string.IsNullOrWhiteSpace(orderNumber))
            throw new ArgumentException("Order number is required", nameof(orderNumber));

        if (string.IsNullOrWhiteSpace(vendorName))
            throw new ArgumentException("Vendor name is required", nameof(vendorName));

        OrderNumber = orderNumber.Trim();
        OrderDate = orderDate;
        ExpectedDeliveryDate = expectedDeliveryDate;
        VendorId = vendorId;
        VendorName = vendorName.Trim();
        TotalAmount = 0;
        ReceivedAmount = 0;
        BilledAmount = 0;
        Status = PurchaseOrderStatus.Draft;
        ApprovalStatus = ApprovalStatus.Pending;
        IsFullyReceived = false;
        IsFullyBilled = false;
        RequesterId = requesterId;
        RequesterName = requesterName?.Trim();
        CostCenterId = costCenterId;
        ProjectId = projectId;
        ShipToAddress = shipToAddress?.Trim();
        PaymentTerms = paymentTerms?.Trim();
        ReferenceNumber = referenceNumber?.Trim();
        Description = description?.Trim();
        Notes = notes?.Trim();

        QueueDomainEvent(new PurchaseOrderCreated(Id, OrderNumber, OrderDate, VendorId, VendorName));
    }

    /// <summary>
    /// Create a new purchase order.
    /// </summary>
    public static PurchaseOrder Create(
        string orderNumber,
        DateTime orderDate,
        DefaultIdType vendorId,
        string vendorName,
        DefaultIdType? requesterId = null,
        string? requesterName = null,
        DefaultIdType? costCenterId = null,
        DefaultIdType? projectId = null,
        DateTime? expectedDeliveryDate = null,
        string? shipToAddress = null,
        string? paymentTerms = null,
        string? referenceNumber = null,
        string? description = null,
        string? notes = null)
    {
        return new PurchaseOrder(orderNumber, orderDate, vendorId, vendorName, requesterId,
            requesterName, costCenterId, projectId, expectedDeliveryDate, shipToAddress,
            paymentTerms, referenceNumber, description, notes);
    }

    /// <summary>
    /// Update purchase order details (only in Draft status).
    /// </summary>
    public void Update(
        DateTime? expectedDeliveryDate = null,
        string? shipToAddress = null,
        string? paymentTerms = null,
        string? description = null,
        string? notes = null)
    {
        if (Status != PurchaseOrderStatus.Draft)
            throw new PurchaseOrderCannotBeModifiedException(Id);

        ExpectedDeliveryDate = expectedDeliveryDate;
        ShipToAddress = shipToAddress?.Trim();
        PaymentTerms = paymentTerms?.Trim();
        Description = description?.Trim();
        Notes = notes?.Trim();

        QueueDomainEvent(new PurchaseOrderUpdated(Id));
    }

    /// <summary>
    /// Set the total amount (typically calculated from line items).
    /// </summary>
    public void SetTotalAmount(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentException("Total amount cannot be negative", nameof(amount));

        TotalAmount = amount;
    }

    /// <summary>
    /// Approve the purchase order.
    /// </summary>
    public void Approve(string approvedBy)
    {
        if (Status != PurchaseOrderStatus.Draft)
            throw new InvalidPurchaseOrderStatusException($"Cannot approve PO with status {Status}");

        if (ApprovalStatus == ApprovalStatus.Approved)
            throw new PurchaseOrderAlreadyApprovedException(Id);

        if (TotalAmount <= 0)
            throw new InvalidPurchaseOrderAmountException("Cannot approve PO with zero amount");

        ApprovalStatus = ApprovalStatus.Approved;
        Status = PurchaseOrderStatus.Approved;
        ApprovedBy = approvedBy?.Trim();
        ApprovedDate = DateTime.UtcNow;

        QueueDomainEvent(new PurchaseOrderApproved(Id, approvedBy ?? string.Empty, ApprovedDate.Value));
    }

    /// <summary>
    /// Reject the purchase order.
    /// </summary>
    public void Reject(string rejectedBy, string? reason = null)
    {
        if (Status != PurchaseOrderStatus.Draft)
            throw new InvalidPurchaseOrderStatusException($"Cannot reject PO with status {Status}");

        ApprovalStatus = ApprovalStatus.Rejected;

        if (!string.IsNullOrWhiteSpace(reason))
        {
            Notes = string.IsNullOrWhiteSpace(Notes) 
                ? $"Rejected by {rejectedBy}: {reason}" 
                : $"{Notes}\nRejected by {rejectedBy}: {reason}";
        }

        QueueDomainEvent(new PurchaseOrderRejected(Id, rejectedBy, reason));
    }

    /// <summary>
    /// Mark purchase order as sent to vendor.
    /// </summary>
    public void MarkAsSent()
    {
        if (Status != PurchaseOrderStatus.Approved)
            throw new InvalidPurchaseOrderStatusException($"Cannot send PO with status {Status}");

        Status = PurchaseOrderStatus.Sent;
        QueueDomainEvent(new PurchaseOrderSent(Id));
    }

    /// <summary>
    /// Record receipt of goods/services.
    /// </summary>
    public void RecordReceipt(decimal receivedAmount)
    {
        if (Status == PurchaseOrderStatus.Draft || Status == PurchaseOrderStatus.Cancelled)
            throw new InvalidPurchaseOrderStatusException($"Cannot record receipt for PO with status {Status}");

        if (receivedAmount < 0)
            throw new ArgumentException("Received amount cannot be negative", nameof(receivedAmount));

        ReceivedAmount += receivedAmount;

        if (ReceivedAmount >= TotalAmount)
        {
            IsFullyReceived = true;
            Status = PurchaseOrderStatus.Received;
        }
        else if (ReceivedAmount > 0)
        {
            Status = PurchaseOrderStatus.PartiallyReceived;
        }

        QueueDomainEvent(new PurchaseOrderReceiptRecorded(Id, receivedAmount, ReceivedAmount, IsFullyReceived));
    }

    /// <summary>
    /// Record invoice matching (three-way match).
    /// </summary>
    public void RecordInvoiceMatch(DefaultIdType invoiceId, decimal billedAmount)
    {
        if (!IsFullyReceived && Status != PurchaseOrderStatus.PartiallyReceived)
            throw new PurchaseOrderNotReceivedException(Id);

        if (billedAmount < 0)
            throw new ArgumentException("Billed amount cannot be negative", nameof(billedAmount));

        if (BilledAmount + billedAmount > ReceivedAmount)
            throw new PurchaseOrderBilledAmountExceedsReceivedException(Id);

        BilledAmount += billedAmount;

        if (BilledAmount >= ReceivedAmount)
        {
            IsFullyBilled = true;
        }

        QueueDomainEvent(new PurchaseOrderInvoiceMatched(Id, invoiceId, billedAmount, BilledAmount, IsFullyBilled));
    }

    /// <summary>
    /// Close the purchase order (manually or automatically when fully processed).
    /// </summary>
    public void Close(string? reason = null)
    {
        if (Status == PurchaseOrderStatus.Closed || Status == PurchaseOrderStatus.Cancelled)
            throw new InvalidPurchaseOrderStatusException($"PO is already {Status}");

        Status = PurchaseOrderStatus.Closed;

        if (!string.IsNullOrWhiteSpace(reason))
        {
            Notes = string.IsNullOrWhiteSpace(Notes) 
                ? $"Closed: {reason}" 
                : $"{Notes}\nClosed: {reason}";
        }

        QueueDomainEvent(new PurchaseOrderClosed(Id, reason));
    }

    /// <summary>
    /// Cancel the purchase order.
    /// </summary>
    public void Cancel(string cancelledBy, string? reason = null)
    {
        if (Status == PurchaseOrderStatus.Closed || Status == PurchaseOrderStatus.Cancelled)
            throw new InvalidPurchaseOrderStatusException($"Cannot cancel PO with status {Status}");

        if (ReceivedAmount > 0)
            throw new PurchaseOrderCannotBeCancelledException(Id, "Cannot cancel PO with received items");

        Status = PurchaseOrderStatus.Cancelled;

        if (!string.IsNullOrWhiteSpace(reason))
        {
            Notes = string.IsNullOrWhiteSpace(Notes) 
                ? $"Cancelled by {cancelledBy}: {reason}" 
                : $"{Notes}\nCancelled by {cancelledBy}: {reason}";
        }

        QueueDomainEvent(new PurchaseOrderCancelled(Id, cancelledBy, reason));
    }
}

/// <summary>
/// Purchase order status values.
/// </summary>
public enum PurchaseOrderStatus
{
    Draft,
    Approved,
    Sent,
    PartiallyReceived,
    Received,
    Closed,
    Cancelled
}

/// <summary>
/// Approval status values.
/// </summary>
public enum ApprovalStatus
{
    Pending,
    Approved,
    Rejected
}
