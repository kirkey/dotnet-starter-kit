namespace Accounting.Domain.Events.Bill;

public record BillCreated(DefaultIdType Id, string BillNumber, DefaultIdType VendorId, string VendorInvoiceNumber, DateTime BillDate, decimal TotalAmount, string? Description, string? Notes) : DomainEvent;

public record BillUpdated(DefaultIdType Id, string BillNumber, decimal TotalAmount, string? Description, string? Notes) : DomainEvent;

public record BillSubmittedForApproval(DefaultIdType Id, string BillNumber, DefaultIdType VendorId, decimal TotalAmount) : DomainEvent;

public record BillApproved(DefaultIdType Id, string BillNumber, string ApprovedBy, DateTime ApprovalDate, decimal TotalAmount) : DomainEvent;

public record BillRejected(DefaultIdType Id, string BillNumber, string RejectionReason) : DomainEvent;

public record BillPaymentApplied(DefaultIdType Id, string BillNumber, decimal PaymentAmount, decimal TotalPaid, decimal OutstandingAmount, DateTime PaymentDate) : DomainEvent;

public record BillVoided(DefaultIdType Id, string BillNumber, string Reason) : DomainEvent;

public record BillDeleted(DefaultIdType Id) : DomainEvent;

