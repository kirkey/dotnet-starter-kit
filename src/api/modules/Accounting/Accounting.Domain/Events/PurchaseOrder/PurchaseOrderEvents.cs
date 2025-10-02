namespace Accounting.Domain.Events.PurchaseOrder;

public sealed record PurchaseOrderCreated(
    DefaultIdType Id,
    string OrderNumber,
    DateTime OrderDate,
    DefaultIdType VendorId,
    string VendorName) : DomainEvent;

public sealed record PurchaseOrderUpdated(DefaultIdType Id) : DomainEvent;

public sealed record PurchaseOrderApproved(
    DefaultIdType Id,
    string ApprovedBy,
    DateTime ApprovedDate) : DomainEvent;

public sealed record PurchaseOrderRejected(
    DefaultIdType Id,
    string RejectedBy,
    string? Reason) : DomainEvent;

public sealed record PurchaseOrderSent(DefaultIdType Id) : DomainEvent;

public sealed record PurchaseOrderReceiptRecorded(
    DefaultIdType Id,
    decimal ReceivedAmount,
    decimal TotalReceivedAmount,
    bool IsFullyReceived) : DomainEvent;

public sealed record PurchaseOrderInvoiceMatched(
    DefaultIdType Id,
    DefaultIdType InvoiceId,
    decimal BilledAmount,
    decimal TotalBilledAmount,
    bool IsFullyBilled) : DomainEvent;

public sealed record PurchaseOrderClosed(
    DefaultIdType Id,
    string? Reason) : DomainEvent;

public sealed record PurchaseOrderCancelled(
    DefaultIdType Id,
    string CancelledBy,
    string? Reason) : DomainEvent;

public sealed record PurchaseOrderDeleted(DefaultIdType Id) : DomainEvent;
