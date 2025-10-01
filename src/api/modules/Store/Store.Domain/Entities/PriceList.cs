namespace Store.Domain;

/// <summary>
/// Represents a named price list used to price grocery items (retail, wholesale, promotional).
/// </summary>
/// <remarks>
/// Use cases:
/// - Apply different prices per customer type or promotion period.
/// - Activate/deactivate price lists by date range.
/// - Support multi-tier pricing strategies (retail vs wholesale).
/// - Enable seasonal or promotional pricing campaigns.
/// - Maintain currency-specific pricing for international operations.
/// </remarks>
/// <seealso cref="Store.Domain.Events.PriceListCreated"/>
/// <seealso cref="Store.Domain.Events.PriceListUpdated"/>
/// <seealso cref="Store.Domain.Events.PriceListActivated"/>
/// <seealso cref="Store.Domain.Events.PriceListDeactivated"/>
/// <seealso cref="Store.Domain.Exceptions.PriceList.PriceListNotFoundException"/>
public sealed class PriceList : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Friendly name of the price list. Example: "Retail Standard", "Wholesale Q4 2025".
    /// Max length: 200.
    /// </summary>
    public string PriceListName { get; private set; } = default!;

    /// <summary>
    /// Type to classify the list: Retail, Wholesale, Promotional, Seasonal.
    /// Example: "Retail" for standard customer pricing, "Promotional" for sale events.
    /// </summary>
    public string PriceListType { get; private set; } = default!; // Retail, Wholesale, Promotional, Seasonal

    /// <summary>
    /// Date when the price list becomes effective.
    /// Example: 2025-10-01T00:00:00Z for Q4 pricing.
    /// </summary>
    public DateTime EffectiveDate { get; private set; }

    /// <summary>
    /// Optional expiry date after which the price list is no longer valid.
    /// Example: 2025-12-31T23:59:59Z for end-of-year expiration.
    /// </summary>
    public DateTime? ExpiryDate { get; private set; }

    /// <summary>
    /// Whether the price list is active. Default: true.
    /// Used to enable/disable price lists without deleting them.
    /// </summary>
    public bool IsActive { get; private set; } = true;

    /// <summary>
    /// ISO currency code for list prices. Default: "USD".
    /// Example: "USD", "EUR", "GBP" for international pricing.
    /// </summary>
    public string Currency { get; private set; } = default!;

    /// <summary>
    /// Minimum order value to qualify for this price list. Null if not applicable.
    /// Example: 500.00 for wholesale minimum order requirements.
    /// </summary>
    public decimal? MinimumOrderValue { get; private set; }

    /// <summary>
    /// Optional customer type for targeted price lists (e.g., VIP).
    /// Example: "VIP", "Wholesale", "Employee" for customer-specific pricing.
    /// </summary>
    public string? CustomerType { get; private set; } // Retail, Wholesale, VIP, etc.
    
    
    /// <summary>
    /// Navigation property to price list items containing item-specific pricing.
    /// </summary>
    public ICollection<PriceListItem> Items { get; private set; } = new List<PriceListItem>();

    private PriceList() { }

    private PriceList(
        DefaultIdType id,
        string name,
        string? description,
        string priceListName,
        string priceListType,
        DateTime effectiveDate,
        DateTime? expiryDate,
        string currency,
        decimal? minimumOrderValue,
        string? customerType,
        string? notes)
    {
        // validations
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required", nameof(name));
        if (name.Length > 200) throw new ArgumentException("Name must not exceed 200 characters", nameof(name));

        if (string.IsNullOrWhiteSpace(priceListName)) throw new ArgumentException("PriceListName is required", nameof(priceListName));
        if (priceListName.Length > 200) throw new ArgumentException("PriceListName must not exceed 200 characters", nameof(priceListName));

        if (string.IsNullOrWhiteSpace(priceListType)) throw new ArgumentException("PriceListType is required", nameof(priceListType));
        if (priceListType.Length > 50) throw new ArgumentException("PriceListType must not exceed 50 characters", nameof(priceListType));

        if (expiryDate.HasValue && expiryDate.Value < effectiveDate) throw new ArgumentException("ExpiryDate cannot be earlier than EffectiveDate", nameof(expiryDate));

        if (string.IsNullOrWhiteSpace(currency)) throw new ArgumentException("Currency is required", nameof(currency));
        if (currency.Length > 10) throw new ArgumentException("Currency must not exceed 10 characters", nameof(currency));

        if (minimumOrderValue.HasValue && minimumOrderValue.Value < 0m) throw new ArgumentException("MinimumOrderValue must be zero or greater", nameof(minimumOrderValue));

        Id = id;
        Name = name;
        Description = description;
        PriceListName = priceListName;
        PriceListType = priceListType;
        EffectiveDate = effectiveDate;
        ExpiryDate = expiryDate;
        IsActive = true;
        Currency = currency;
        MinimumOrderValue = minimumOrderValue;
        CustomerType = customerType;
        Notes = notes;

        QueueDomainEvent(new PriceListCreated { PriceList = this });
    }

    /// <summary>
    /// Factory method to create a new PriceList instance.
    /// </summary>
    /// <param name="name">The name of the price list.</param>
    /// <param name="description">Optional description of the price list.</param>
    /// <param name="priceListName">The friendly name of the price list.</param>
    /// <param name="priceListType">The type of the price list (e.g., Retail, Wholesale).</param>
    /// <param name="effectiveDate">The date when the price list becomes effective.</param>
    /// <param name="expiryDate">Optional expiry date for the price list.</param>
    /// <param name="currency">The ISO currency code for the price list. Default is "USD".</param>
    /// <param name="minimumOrderValue">Optional minimum order value to qualify for this price list.</param>
    /// <param name="customerType">Optional customer type for targeted price lists (e.g., VIP).</param>
    /// <param name="notes">Optional notes for internal use.</param>
    /// <returns>A new instance of the <see cref="PriceList"/> class.</returns>
    public static PriceList Create(
        string name,
        string? description,
        string priceListName,
        string priceListType,
        DateTime effectiveDate,
        DateTime? expiryDate,
        string currency = "USD",
        decimal? minimumOrderValue = null,
        string? customerType = null,
        string? notes = null)
    {
        return new PriceList(
            DefaultIdType.NewGuid(),
            name,
            description,
            priceListName,
            priceListType,
            effectiveDate,
            expiryDate,
            currency,
            minimumOrderValue,
            customerType,
            notes);
    }

    /// <summary>
    /// Adds an item to the price list with the specified pricing details.
    /// </summary>
    /// <param name="groceryItemId">The identifier of the grocery item.</param>
    /// <param name="price">The price of the item in this price list.</param>
    /// <param name="discountPercentage">Optional discount percentage for the item.</param>
    /// <returns>The updated <see cref="PriceList"/> instance.</returns>
    public PriceList AddItem(DefaultIdType groceryItemId, decimal price, decimal? discountPercentage = null)
    {
        var item = PriceListItem.Create(Id, groceryItemId, price, discountPercentage);
        Items.Add(item);
        return this;
    }

    /// <summary>
    /// Activates the price list, making it available for use.
    /// </summary>
    /// <returns>The updated <see cref="PriceList"/> instance.</returns>
    public PriceList Activate()
    {
        if (!IsActive)
        {
            IsActive = true;
            QueueDomainEvent(new PriceListActivated { PriceList = this });
        }
        return this;
    }

    /// <summary>
    /// Deactivates the price list, preventing its use until reactivated.
    /// </summary>
    /// <returns>The updated <see cref="PriceList"/> instance.</returns>
    public PriceList Deactivate()
    {
        if (IsActive)
        {
            IsActive = false;
            QueueDomainEvent(new PriceListDeactivated { PriceList = this });
        }
        return this;
    }

    /// <summary>
    /// Checks if the price list is currently active based on the effective and expiry dates.
    /// </summary>
    /// <returns>True if the price list is active, false otherwise.</returns>
    public bool IsCurrentlyActive() => 
        IsActive && DateTime.UtcNow >= EffectiveDate && (!ExpiryDate.HasValue || DateTime.UtcNow <= ExpiryDate.Value);
    
    /// <summary>
    /// Checks if the price list has expired.
    /// </summary>
    /// <returns>True if the price list is expired, false otherwise.</returns>
    public bool IsExpired() => ExpiryDate.HasValue && DateTime.UtcNow > ExpiryDate.Value;

    /// <summary>
    /// Updates the price list with the specified details.
    /// </summary>
    /// <param name="name">The new name of the price list.</param>
    /// <param name="description">Optional new description of the price list.</param>
    /// <param name="priceListName">The new friendly name of the price list.</param>
    /// <param name="priceListType">The new type of the price list (e.g., Retail, Wholesale).</param>
    /// <param name="effectiveDate">The new date when the price list becomes effective.</param>
    /// <param name="expiryDate">Optional new expiry date for the price list.</param>
    /// <param name="isActive">The new active status of the price list.</param>
    /// <param name="currency">The new ISO currency code for the price list.</param>
    /// <param name="minimumOrderValue">Optional new minimum order value to qualify for this price list.</param>
    /// <param name="customerType">Optional new customer type for targeted price lists (e.g., VIP).</param>
    /// <param name="notes">Optional new notes for internal use.</param>
    /// <returns>The updated <see cref="PriceList"/> instance.</returns>
    public PriceList Update(
        string name,
        string? description,
        string priceListName,
        string priceListType,
        DateTime effectiveDate,
        DateTime? expiryDate,
        bool isActive,
        string currency,
        decimal? minimumOrderValue,
        string? customerType,
        string? notes)
    {
        var changed = false;

        if (Name != name)
        {
            Name = name;
            changed = true;
        }

        if (Description != description)
        {
            Description = description;
            changed = true;
        }

        if (PriceListName != priceListName)
        {
            PriceListName = priceListName;
            changed = true;
        }

        if (PriceListType != priceListType)
        {
            PriceListType = priceListType;
            changed = true;
        }

        if (EffectiveDate != effectiveDate)
        {
            EffectiveDate = effectiveDate;
            changed = true;
        }

        if (ExpiryDate != expiryDate)
        {
            ExpiryDate = expiryDate;
            changed = true;
        }

        if (IsActive != isActive)
        {
            IsActive = isActive;
            changed = true;
        }

        if (Currency != currency)
        {
            Currency = currency;
            changed = true;
        }

        if (MinimumOrderValue != minimumOrderValue)
        {
            MinimumOrderValue = minimumOrderValue;
            changed = true;
        }

        if (CustomerType != customerType)
        {
            CustomerType = customerType;
            changed = true;
        }

        if (Notes != notes)
        {
            Notes = notes;
            changed = true;
        }

        if (changed)
        {
            QueueDomainEvent(new PriceListUpdated { PriceList = this });
        }

        return this;
    }
}
