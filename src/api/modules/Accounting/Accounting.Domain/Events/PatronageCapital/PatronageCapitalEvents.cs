namespace Accounting.Domain.Events.PatronageCapital;

/// <summary>
/// Event raised when patronage capital is allocated to a member.
/// </summary>
public record PatronageCapitalAllocated(
    DefaultIdType Id,
    DefaultIdType MemberId,
    int FiscalYear,
    decimal AmountAllocated,
    DateTime AllocationDate) : DomainEvent;

/// <summary>
/// Event raised when patronage capital is updated.
/// </summary>
public record PatronageCapitalUpdated(Accounting.Domain.PatronageCapital PatronageCapital) : DomainEvent;

/// <summary>
/// Event raised when patronage capital is deleted.
/// </summary>
public record PatronageCapitalDeleted(DefaultIdType Id, DefaultIdType MemberId, int FiscalYear) : DomainEvent;

/// <summary>
/// Event raised when patronage capital is fully retired.
/// </summary>
public record PatronageCapitalRetired(
    DefaultIdType Id,
    DefaultIdType MemberId,
    int FiscalYear,
    decimal AmountRetired,
    DateTime RetirementDate,
    string? RetirementMethod) : DomainEvent;

/// <summary>
/// Event raised when patronage capital is partially retired.
/// </summary>
public record PatronageCapitalPartiallyRetired(
    DefaultIdType Id,
    DefaultIdType MemberId,
    int FiscalYear,
    decimal AmountRetired,
    decimal RemainingAmount,
    DateTime RetirementDate,
    string? RetirementMethod) : DomainEvent;

/// <summary>
/// Event raised when patronage capital is transferred between members.
/// </summary>
public record PatronageCapitalTransferred(
    DefaultIdType Id,
    DefaultIdType FromMemberId,
    DefaultIdType ToMemberId,
    int FiscalYear,
    decimal Amount,
    string? TransferReason) : DomainEvent;
