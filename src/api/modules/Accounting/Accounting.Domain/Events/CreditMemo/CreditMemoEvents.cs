namespace Accounting.Domain.Events.CreditMemo;

/// <summary>
/// Event raised when a credit memo is created.
/// </summary>
public record CreditMemoCreated(
    DefaultIdType Id,
    string MemoNumber,
    DateTime MemoDate,
    decimal Amount,
    string ReferenceType,
    DefaultIdType ReferenceId,
    string? Reason) : DomainEvent;

/// <summary>
/// Event raised when a credit memo is updated.
/// </summary>
public record CreditMemoUpdated(Accounting.Domain.CreditMemo CreditMemo) : DomainEvent;

/// <summary>
/// Event raised when a credit memo is deleted.
/// </summary>
public record CreditMemoDeleted(DefaultIdType Id, string MemoNumber) : DomainEvent;

/// <summary>
/// Event raised when a credit memo is approved.
/// </summary>
public record CreditMemoApproved(
    DefaultIdType Id,
    string MemoNumber,
    string ApprovedBy,
    DateTime ApprovedDate) : DomainEvent;

/// <summary>
/// Event raised when a credit memo is applied to an invoice or bill.
/// </summary>
public record CreditMemoApplied(
    DefaultIdType Id,
    string MemoNumber,
    DefaultIdType TargetDocumentId,
    decimal AmountApplied,
    decimal RemainingBalance) : DomainEvent;

/// <summary>
/// Event raised when a credit memo is refunded.
/// </summary>
public record CreditMemoRefunded(
    DefaultIdType Id,
    string MemoNumber,
    decimal RefundAmount,
    DateTime RefundDate,
    string? RefundMethod,
    string? RefundReference) : DomainEvent;

/// <summary>
/// Event raised when a credit memo is voided.
/// </summary>
public record CreditMemoVoided(
    DefaultIdType Id,
    string MemoNumber,
    DateTime VoidDate,
    string? VoidReason) : DomainEvent;

/// <summary>
/// Event raised when a credit memo is rejected.
/// </summary>
public record CreditMemoRejected(
    DefaultIdType Id,
    string MemoNumber,
    string RejectedBy,
    DateTime RejectedDate,
    string? RejectionReason) : DomainEvent;
