using Accounting.Domain.Events.Currency;
using Accounting.Domain.Exceptions;
using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;

namespace Accounting.Domain;

public class Currency : AuditableEntity, IAggregateRoot
{
    public string CurrencyCode { get; private set; } // ISO 4217 code (USD, EUR, JPY)
    public string Symbol { get; private set; }
    public int DecimalPlaces { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsBaseCurrency { get; private set; }

    private Currency(string currencyCode, string currencyName, string symbol, int decimalPlaces = 2, string? description = null, string? notes = null)
    {
        CurrencyCode = currencyCode.Trim().ToUpper();
        Name = currencyName.Trim();
        Symbol = symbol.Trim();
        DecimalPlaces = decimalPlaces;
        IsActive = true;
        IsBaseCurrency = false;
        Description = description?.Trim();
        Notes = notes?.Trim();

        QueueDomainEvent(new CurrencyCreated(Id, CurrencyCode, Name, Symbol, Description, Notes));
    }

    public static Currency Create(string currencyCode, string currencyName, string symbol, int decimalPlaces = 2, string? description = null, string? notes = null)
    {
        if (string.IsNullOrWhiteSpace(currencyCode) || currencyCode.Length != 3)
            throw new InvalidCurrencyCodeException(currencyCode);

        if (decimalPlaces < 0 || decimalPlaces > 4)
            throw new InvalidCurrencyDecimalPlacesException(decimalPlaces);

        return new Currency(currencyCode, currencyName, symbol, decimalPlaces, description, notes);
    }

    public Currency Update(string? currencyCode, string? currencyName, string? symbol, int? decimalPlaces, bool? isActive, string? description, string? notes)
    {
        bool isUpdated = false;
        
        if (!string.IsNullOrWhiteSpace(currencyCode) && !string.Equals(CurrencyCode, currencyCode.Trim(), StringComparison.OrdinalIgnoreCase))
        {
            if (currencyCode.Length != 3)
                throw new InvalidCurrencyCodeException(currencyCode);
            CurrencyCode = currencyCode.Trim().ToUpper();
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(currencyName) && Name != currencyName)
        {
            Name = currencyName.Trim();
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(symbol) && Symbol != symbol)
        {
            Symbol = symbol.Trim();
            isUpdated = true;
        }

        if (decimalPlaces.HasValue && DecimalPlaces != decimalPlaces.Value)
        {
            if (decimalPlaces.Value < 0 || decimalPlaces.Value > 4)
                throw new InvalidCurrencyDecimalPlacesException(decimalPlaces.Value);
            DecimalPlaces = decimalPlaces.Value;
            isUpdated = true;
        }

        if (isActive.HasValue && IsActive != isActive.Value)
        {
            IsActive = isActive.Value;
            isUpdated = true;
        }

        if (description != Description)
        {
            Description = description?.Trim();
            isUpdated = true;
        }

        if (notes != Notes)
        {
            Notes = notes?.Trim();
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new CurrencyUpdated(this));
        }

        return this;
    }

    public Currency SetAsBaseCurrency()
    {
        if (IsBaseCurrency)
            throw new CurrencyAlreadyBaseCurrencyException(Id);

        IsBaseCurrency = true;
        QueueDomainEvent(new CurrencySetAsBase(Id, CurrencyCode));
        return this;
    }

    public Currency RemoveAsBaseCurrency()
    {
        if (!IsBaseCurrency)
            throw new CurrencyNotBaseCurrencyException(Id);

        IsBaseCurrency = false;
        QueueDomainEvent(new CurrencyRemovedAsBase(Id, CurrencyCode));
        return this;
    }

    public Currency Activate()
    {
        if (IsActive)
            throw new CurrencyAlreadyActiveException(Id);

        IsActive = true;
        QueueDomainEvent(new CurrencyActivated(Id, CurrencyCode));
        return this;
    }

    public Currency Deactivate()
    {
        if (!IsActive)
            throw new CurrencyAlreadyInactiveException(Id);

        if (IsBaseCurrency)
            throw new CannotDeactivateBaseCurrencyException(Id);

        IsActive = false;
        QueueDomainEvent(new CurrencyDeactivated(Id, CurrencyCode));
        return this;
    }
}

public class ExchangeRate : AuditableEntity, IAggregateRoot
{
    public DefaultIdType FromCurrencyId { get; private set; }
    public DefaultIdType ToCurrencyId { get; private set; }
    public DateTime EffectiveDate { get; private set; }
    public decimal Rate { get; private set; }
    public string? Source { get; private set; } // e.g., "Central Bank", "Manual Entry"
    public bool IsActive { get; private set; }

    private ExchangeRate(DefaultIdType fromCurrencyId, DefaultIdType toCurrencyId,
        DateTime effectiveDate, decimal rate, string? source = null)
    {
        FromCurrencyId = fromCurrencyId;
        ToCurrencyId = toCurrencyId;
        EffectiveDate = effectiveDate;
        Rate = rate;
        Source = source?.Trim();
        IsActive = true;

        QueueDomainEvent(new ExchangeRateCreated(Id, FromCurrencyId, ToCurrencyId, EffectiveDate, Rate));
    }

    public static ExchangeRate Create(DefaultIdType fromCurrencyId, DefaultIdType toCurrencyId,
        DateTime effectiveDate, decimal rate, string? source = null)
    {
        if (fromCurrencyId == toCurrencyId)
            throw new SameCurrencyExchangeRateException();

        if (rate <= 0)
            throw new InvalidExchangeRateException(rate);

        return new ExchangeRate(fromCurrencyId, toCurrencyId, effectiveDate, rate, source);
    }

    public ExchangeRate Update(DateTime? effectiveDate, decimal? rate, string? source, bool? isActive)
    {
        bool isUpdated = false;

        if (effectiveDate.HasValue && EffectiveDate != effectiveDate.Value)
        {
            EffectiveDate = effectiveDate.Value;
            isUpdated = true;
        }

        if (rate.HasValue && Rate != rate.Value)
        {
            if (rate.Value <= 0)
                throw new InvalidExchangeRateException(rate.Value);
            Rate = rate.Value;
            isUpdated = true;
        }

        if (source != Source)
        {
            Source = source?.Trim();
            isUpdated = true;
        }

        if (isActive.HasValue && IsActive != isActive.Value)
        {
            IsActive = isActive.Value;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new ExchangeRateUpdated(this));
        }

        return this;
    }

    public ExchangeRate Activate()
    {
        if (IsActive)
            throw new ExchangeRateAlreadyActiveException(Id);

        IsActive = true;
        QueueDomainEvent(new ExchangeRateActivated(Id, FromCurrencyId, ToCurrencyId));
        return this;
    }

    public ExchangeRate Deactivate()
    {
        if (!IsActive)
            throw new ExchangeRateAlreadyInactiveException(Id);

        IsActive = false;
        QueueDomainEvent(new ExchangeRateDeactivated(Id, FromCurrencyId, ToCurrencyId));
        return this;
    }

    public decimal ConvertAmount(decimal amount)
    {
        return amount * Rate;
    }

    public decimal ConvertAmountReverse(decimal amount)
    {
        return amount / Rate;
    }
}
