namespace Accounting.Domain.Events.PaymentAllocation;

/// <summary>
/// Event raised when a payment allocation is created.
/// </summary>
public record PaymentAllocationCreated(
    DefaultIdType Id,
    DefaultIdType PaymentId,
    DefaultIdType InvoiceId,
    decimal Amount,
    DateTime AllocationDate,
    string? Notes) : DomainEvent;

/// <summary>
/// Event raised when a payment allocation is updated.
/// </summary>
public record PaymentAllocationUpdated(Entities.PaymentAllocation PaymentAllocation) : DomainEvent;

/// <summary>
/// Event raised when a payment allocation is deleted.
/// </summary>
public record PaymentAllocationDeleted(
    DefaultIdType Id,
    DefaultIdType PaymentId,
    DefaultIdType InvoiceId,
    decimal Amount) : DomainEvent;

/// <summary>
/// Event raised when a payment allocation amount is adjusted.
/// </summary>
public record PaymentAllocationAmountAdjusted(
    DefaultIdType Id,
    DefaultIdType PaymentId,
    DefaultIdType InvoiceId,
    decimal OldAmount,
    decimal NewAmount,
    string? Reason) : DomainEvent;

/// <summary>
/// Event raised when a payment allocation is reversed.
/// </summary>
public record PaymentAllocationReversed(
    DefaultIdType Id,
    DefaultIdType PaymentId,
    DefaultIdType InvoiceId,
    decimal Amount,
    DateTime ReversalDate,
    string? ReversalReason) : DomainEvent;
