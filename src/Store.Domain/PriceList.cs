namespace Store.Domain;

public sealed class PriceList : AuditableEntity, IAggregateRoot
{
    public string PriceListName { get; private set; } = default!;
    public string PriceListType { get; private set; } = default!; // Retail, Wholesale, Promotional, Seasonal
    public DateTime EffectiveDate { get; private set; }
    public DateTime? ExpiryDate { get; private set; }
    public bool IsActive { get; private set; } = true;
    public string Currency { get; private set; } = default!;
    public decimal? MinimumOrderValue { get; private set; }
    public string? CustomerType { get; private set; } // Retail, Wholesale, VIP, etc.
    
    
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

    public PriceList AddItem(DefaultIdType groceryItemId, decimal price, decimal? discountPercentage = null)
    {
        var item = PriceListItem.Create(Id, groceryItemId, price, discountPercentage);
        Items.Add(item);
        return this;
    }

    public PriceList Activate()
    {
        if (!IsActive)
        {
            IsActive = true;
            QueueDomainEvent(new PriceListActivated { PriceList = this });
        }
        return this;
    }

    public PriceList Deactivate()
    {
        if (IsActive)
        {
            IsActive = false;
            QueueDomainEvent(new PriceListDeactivated { PriceList = this });
        }
        return this;
    }

    public bool IsCurrentlyActive() => 
        IsActive && DateTime.UtcNow >= EffectiveDate && (!ExpiryDate.HasValue || DateTime.UtcNow <= ExpiryDate.Value);
    
    public bool IsExpired() => ExpiryDate.HasValue && DateTime.UtcNow > ExpiryDate.Value;

    // Update method to apply editable fields and emit update event when necessary
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
