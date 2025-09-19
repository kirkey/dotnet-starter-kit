namespace Store.Domain;

/// <summary>
/// Represents a sales tax rate that may be applied to items at POS.
/// </summary>
/// <remarks>
/// Use cases:
/// - Define tax rates for different jurisdictions or item types.
/// - Calculate taxes during checkout and invoice generation.
/// - Support both tax-inclusive and tax-exclusive pricing models.
/// - Handle multiple tax rates for complex tax scenarios.
/// </remarks>
/// <seealso cref="Store.Domain.Events.TaxRateCreated"/>
/// <seealso cref="Store.Domain.Events.TaxRateUpdated"/>
public sealed class TaxRate : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Tax rate as a decimal between 0.00 and 1.00.
    /// Example: 0.10 for 10% tax, 0.0825 for 8.25% tax.
    /// </summary>
    public decimal Rate { get; private set; } // 0.00 .. 1.00

    /// <summary>
    /// Whether prices include tax (true) or tax is added on top (false).
    /// Example: true for VAT-inclusive pricing, false for US sales tax model.
    /// Default: false.
    /// </summary>
    public bool IsInclusive { get; private set; } // true if prices include tax

    private TaxRate() { }

    private TaxRate(DefaultIdType id, string name, decimal rate, bool isInclusive)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required", nameof(name));
        if (name.Length > 100) throw new ArgumentException("Name must not exceed 100 characters", nameof(name));
        if (rate is < 0m or > 1m) throw new ArgumentException("Rate must be between 0.00 and 1.00", nameof(rate));

        Id = id;
        Name = name;
        Rate = rate;
        IsInclusive = isInclusive;
        QueueDomainEvent(new TaxRateCreated { TaxRate = this });
    }

    /// <summary>
    /// Factory to create a new tax rate.
    /// </summary>
    /// <param name="name">Tax rate name. Example: "Standard VAT".</param>
    /// <param name="rate">Rate as decimal (0.00-1.00). Example: 0.10 for 10%.</param>
    /// <param name="isInclusive">Whether prices include tax. Default: false.</param>
    public static TaxRate Create(string name, decimal rate, bool isInclusive = false)
        => new(DefaultIdType.NewGuid(), name, rate, isInclusive);

    /// <summary>
    /// Updates tax rate properties.
    /// </summary>
    /// <param name="name">New name if provided.</param>
    /// <param name="rate">New rate if provided.</param>
    /// <param name="isInclusive">New inclusive flag if provided.</param>
    public TaxRate Update(string? name, decimal? rate, bool? isInclusive)
    {
        bool changed = false;
        if (!string.IsNullOrWhiteSpace(name) && !string.Equals(Name, name, StringComparison.OrdinalIgnoreCase))
        {
            if (name.Length > 100) throw new ArgumentException("Name must not exceed 100 characters", nameof(name));
            Name = name; changed = true;
        }
        if (rate.HasValue && Rate != rate.Value)
        {
            if (rate.Value is < 0m or > 1m) throw new ArgumentException("Rate must be between 0.00 and 1.00", nameof(rate));
            Rate = rate.Value; changed = true;
        }
        if (isInclusive.HasValue && IsInclusive != isInclusive.Value)
        {
            IsInclusive = isInclusive.Value; changed = true;
        }
        if (changed) QueueDomainEvent(new TaxRateUpdated { TaxRate = this });
        return this;
    }
}
