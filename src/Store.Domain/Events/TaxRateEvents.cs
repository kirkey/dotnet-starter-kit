namespace Store.Domain.Events;

/// <summary>
/// Event raised when a tax rate is deleted.
/// </summary>
public record TaxRateDeleted(DefaultIdType Id, string Name) : DomainEvent;

/// <summary>
/// Event raised when a tax rate percentage is changed.
/// </summary>
public record TaxRateChanged(
    DefaultIdType Id,
    string Name,
    decimal OldRate,
    decimal NewRate) : DomainEvent;

/// <summary>
/// Event raised when tax inclusivity setting is changed.
/// </summary>
public record TaxInclusivityChanged(
    DefaultIdType Id,
    string Name,
    bool OldIsInclusive,
    bool NewIsInclusive) : DomainEvent;
