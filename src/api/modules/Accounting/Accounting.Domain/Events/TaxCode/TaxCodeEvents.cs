namespace Accounting.Domain.Events.TaxCode;

public sealed record TaxCodeCreated(
    DefaultIdType Id,
    string Code,
    string Name,
    TaxType TaxType,
    decimal Rate,
    DateTime EffectiveDate) : DomainEvent;

public sealed record TaxCodeUpdated(
    DefaultIdType Id,
    string Name) : DomainEvent;

public sealed record TaxCodeRateUpdated(
    DefaultIdType Id,
    decimal NewRate,
    DateTime EffectiveDate) : DomainEvent;

public sealed record TaxCodeActivated(DefaultIdType Id) : DomainEvent;

public sealed record TaxCodeDeactivated(DefaultIdType Id) : DomainEvent;

public sealed record TaxCodeDeleted(DefaultIdType Id) : DomainEvent;
