using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using Store.Domain.Events;

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
}
