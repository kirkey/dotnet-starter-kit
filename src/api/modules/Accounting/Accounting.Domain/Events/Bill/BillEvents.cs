namespace Accounting.Domain.Events.Bill;

/// <summary>
/// Event raised when a new bill is created.
/// </summary>
public sealed record BillCreated(
    DefaultIdType BillId,
    string BillNumber,
    DefaultIdType VendorId,
    DateTime BillDate,
    DateTime DueDate) : DomainEvent;

/// <summary>
/// Event raised when a bill is updated.
/// </summary>
public sealed record BillUpdated(
    Entities.Bill Bill) : DomainEvent;

/// <summary>
/// Event raised when a bill is approved.
/// </summary>
public sealed record BillApproved(
    DefaultIdType BillId,
    string ApprovedBy) : DomainEvent;

/// <summary>
/// Event raised when a bill is rejected.
/// </summary>
public sealed record BillRejected(
    DefaultIdType BillId,
    string RejectedBy,
    string Reason) : DomainEvent;

/// <summary>
/// Event raised when a bill is posted to the general ledger.
/// </summary>
public sealed record BillPosted(
    DefaultIdType BillId,
    DateTime BillDate,
    decimal TotalAmount) : DomainEvent;

/// <summary>
/// Event raised when a bill is marked as paid.
/// </summary>
public sealed record BillPaid(
    DefaultIdType BillId,
    DateTime PaidDate,
    decimal TotalAmount) : DomainEvent;

/// <summary>
/// Event raised when a bill is voided.
/// </summary>
public sealed record BillVoided(
    DefaultIdType BillId,
    string Reason) : DomainEvent;

