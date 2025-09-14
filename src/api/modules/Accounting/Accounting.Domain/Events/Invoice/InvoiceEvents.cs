using FSH.Framework.Core.Domain.Events;

namespace Accounting.Domain.Events.Invoice;

public record InvoiceCreated(DefaultIdType Id, string InvoiceNumber, DefaultIdType MemberId, decimal TotalAmount, DateTime DueDate, string? Description, string? Notes) : DomainEvent;

public record InvoiceUpdated(DefaultIdType Id, string InvoiceNumber, DefaultIdType MemberId, decimal TotalAmount, string? Description, string? Notes) : DomainEvent;

public record InvoiceDeleted(DefaultIdType Id) : DomainEvent;

public record InvoiceLineItemAdded(DefaultIdType Id, string InvoiceNumber, string Description, decimal Amount) : DomainEvent;

public record InvoiceSent(DefaultIdType Id, string InvoiceNumber, DefaultIdType MemberId, decimal TotalAmount, DateTime DueDate) : DomainEvent;

public record InvoicePaid(DefaultIdType Id, string InvoiceNumber, DefaultIdType MemberId, decimal TotalAmount, DateTime PaidDate, string? PaymentMethod) : DomainEvent;

public record InvoiceOverdue(DefaultIdType Id, string InvoiceNumber, DefaultIdType MemberId, decimal TotalAmount, DateTime DueDate) : DomainEvent;

public record InvoiceCancelled(DefaultIdType Id, string InvoiceNumber, DefaultIdType MemberId, string? Reason) : DomainEvent;

public record InvoicePartiallyPaid(DefaultIdType Id, string InvoiceNumber, DefaultIdType MemberId, decimal AmountApplied, decimal TotalPaidToDate) : DomainEvent;
