using FSH.Framework.Core.Domain.Events;

namespace Accounting.Domain.Events.Payment;

public record PaymentReceived(DefaultIdType PaymentId, string PaymentNumber, DefaultIdType? MemberId, DefaultIdType? InvoiceId, decimal Amount, DateTime PaymentDate, string PaymentMethod) : DomainEvent;
public record PaymentAppliedToInvoice(DefaultIdType PaymentId, string PaymentNumber, DefaultIdType InvoiceId, decimal AmountApplied) : DomainEvent;
public record PaymentReferenceUpdated(DefaultIdType PaymentId, string PaymentNumber, string? ReferenceNumber) : DomainEvent;
public record PaymentDeleted(DefaultIdType PaymentId) : DomainEvent;
