using FSH.Framework.Core.Domain.Events;

namespace Accounting.Domain.Events.Currency;

public record CurrencyCreated(DefaultIdType Id, string CurrencyCode, string CurrencyName, string Symbol, string? Description, string? Notes) : DomainEvent;

public record CurrencyUpdated(Accounting.Domain.Currency Currency) : DomainEvent;

public record CurrencySetAsBase(DefaultIdType Id, string CurrencyCode) : DomainEvent;

public record CurrencyRemovedAsBase(DefaultIdType Id, string CurrencyCode) : DomainEvent;

public record CurrencyActivated(DefaultIdType Id, string CurrencyCode) : DomainEvent;

public record CurrencyDeactivated(DefaultIdType Id, string CurrencyCode) : DomainEvent;

public record ExchangeRateCreated(DefaultIdType Id, DefaultIdType FromCurrencyId, DefaultIdType ToCurrencyId, DateTime EffectiveDate, decimal Rate) : DomainEvent;

public record ExchangeRateUpdated(Accounting.Domain.ExchangeRate ExchangeRate) : DomainEvent;

public record ExchangeRateActivated(DefaultIdType Id, DefaultIdType FromCurrencyId, DefaultIdType ToCurrencyId) : DomainEvent;

public record ExchangeRateDeactivated(DefaultIdType Id, DefaultIdType FromCurrencyId, DefaultIdType ToCurrencyId) : DomainEvent;
