namespace Store.Domain.Events;

/// <summary>
/// Event raised when a POS payment is created.
/// </summary>
public record PosPaymentCreated(
    DefaultIdType Id,
    DefaultIdType PosSaleId,
    string PaymentMethod,
    decimal Amount,
    DateTime PaymentDate,
    string? ReferenceNumber) : DomainEvent;

/// <summary>
/// Event raised when a POS payment is updated.
/// </summary>
public record PosPaymentUpdated(Store.Domain.PosPayment PosPayment) : DomainEvent;

/// <summary>
/// Event raised when a POS payment is deleted.
/// </summary>
public record PosPaymentDeleted(
    DefaultIdType Id,
    DefaultIdType PosSaleId,
    decimal Amount,
    string PaymentMethod) : DomainEvent;

/// <summary>
/// Event raised when a POS payment is voided.
/// </summary>
public record PosPaymentVoided(
    DefaultIdType Id,
    DefaultIdType PosSaleId,
    decimal Amount,
    string PaymentMethod,
    DateTime VoidedDate,
    string? VoidReason) : DomainEvent;

/// <summary>
/// Event raised when a POS payment is refunded.
/// </summary>
public record PosPaymentRefunded(
    DefaultIdType Id,
    DefaultIdType PosSaleId,
    decimal OriginalAmount,
    decimal RefundAmount,
    DateTime RefundDate,
    string? RefundReason) : DomainEvent;
