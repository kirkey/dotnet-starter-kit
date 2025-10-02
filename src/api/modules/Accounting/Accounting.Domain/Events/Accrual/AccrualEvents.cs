namespace Accounting.Domain.Events.Accrual;

/// <summary>
/// Event raised when a new accrual is created.
/// </summary>
public record AccrualCreated(
    DefaultIdType Id, 
    string AccrualNumber, 
    DateTime AccrualDate, 
    decimal Amount, 
    string? Description) : DomainEvent;

/// <summary>
/// Event raised when an accrual is updated.
/// </summary>
public record AccrualUpdated(Entities.Accrual Accrual) : DomainEvent;

/// <summary>
/// Event raised when an accrual is deleted.
/// </summary>
public record AccrualDeleted(DefaultIdType Id, string AccrualNumber) : DomainEvent;

/// <summary>
/// Event raised when an accrual is reversed.
/// </summary>
public record AccrualReversed(
    DefaultIdType Id, 
    string AccrualNumber, 
    DateTime ReversalDate, 
    decimal Amount) : DomainEvent;

/// <summary>
/// Event raised when an accrual amount is adjusted.
/// </summary>
public record AccrualAmountAdjusted(
    DefaultIdType Id, 
    string AccrualNumber, 
    decimal OldAmount, 
    decimal NewAmount, 
    string? Reason) : DomainEvent;
