namespace Accounting.Domain.Events.Payment;

public record PaymentReceived(DefaultIdType PaymentId, string PaymentNumber, DefaultIdType? MemberId, DefaultIdType? InvoiceId, decimal Amount, DateTime PaymentDate, string PaymentMethod) : DomainEvent;
public record PaymentAppliedToInvoice(DefaultIdType PaymentId, string PaymentNumber, DefaultIdType InvoiceId, decimal AmountApplied) : DomainEvent;
public record PaymentReferenceUpdated(DefaultIdType PaymentId, string PaymentNumber, string? ReferenceNumber) : DomainEvent;
public record PaymentUpdated(DefaultIdType PaymentId, string PaymentNumber) : DomainEvent;
public record PaymentRefunded(DefaultIdType PaymentId, string PaymentNumber, DefaultIdType? MemberId, decimal RefundAmount, DateTime RefundDate, string? RefundReference) : DomainEvent;
public record PaymentVoided(DefaultIdType PaymentId, string PaymentNumber, DefaultIdType? MemberId, DateTime VoidDate, string? VoidReason) : DomainEvent;
public record PaymentDeleted(DefaultIdType PaymentId) : DomainEvent;
