namespace Accounting.Domain.Events.DebitMemo;

/// <summary>
/// Event raised when a debit memo is created.
/// </summary>
public record DebitMemoCreated(
    DefaultIdType Id,
    string MemoNumber,
    DateTime MemoDate,
    decimal Amount,
    string ReferenceType,
    DefaultIdType ReferenceId,
    string? Reason) : DomainEvent;

/// <summary>
/// Event raised when a debit memo is updated.
/// </summary>
public record DebitMemoUpdated(Entities.DebitMemo DebitMemo) : DomainEvent;

/// <summary>
/// Event raised when a debit memo is deleted.
/// </summary>
public record DebitMemoDeleted(DefaultIdType Id, string MemoNumber) : DomainEvent;

/// <summary>
/// Event raised when a debit memo is approved.
/// </summary>
public record DebitMemoApproved(
    DefaultIdType Id,
    string MemoNumber,
    string ApprovedBy,
    DateTime ApprovedDate) : DomainEvent;

/// <summary>
/// Event raised when a debit memo is applied to an invoice or bill.
/// </summary>
public record DebitMemoApplied(
    DefaultIdType Id,
    string MemoNumber,
    DefaultIdType TargetDocumentId,
    decimal AmountApplied,
    decimal RemainingBalance) : DomainEvent;

/// <summary>
/// Event raised when a debit memo is voided.
/// </summary>
public record DebitMemoVoided(
    DefaultIdType Id,
    string MemoNumber,
    DateTime VoidDate,
    string? VoidReason) : DomainEvent;

/// <summary>
/// Event raised when a debit memo is rejected.
/// </summary>
public record DebitMemoRejected(
    DefaultIdType Id,
    string MemoNumber,
    string RejectedBy,
    DateTime RejectedDate,
    string? RejectionReason) : DomainEvent;
