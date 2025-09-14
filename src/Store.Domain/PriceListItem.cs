namespace Store.Domain;

public sealed class PriceListItem : AuditableEntity, IAggregateRoot
{
    public DefaultIdType PriceListId { get; private set; }
    public DefaultIdType GroceryItemId { get; private set; }
    public decimal Price { get; private set; }
    public decimal? DiscountPercentage { get; private set; }
    public decimal? MinimumQuantity { get; private set; }
    public decimal? MaximumQuantity { get; private set; }
    public bool IsActive { get; private set; } = true;
    
    public PriceList PriceList { get; private set; } = default!;
    public GroceryItem GroceryItem { get; private set; } = default!;

    private PriceListItem() { }

    private PriceListItem(
        DefaultIdType id,
        DefaultIdType priceListId,
        DefaultIdType groceryItemId,
        decimal price,
        decimal? discountPercentage,
        decimal? minimumQuantity,
        decimal? maximumQuantity)
    {
        // validations
        if (priceListId == default) throw new ArgumentException("PriceListId is required", nameof(priceListId));
        if (groceryItemId == default) throw new ArgumentException("GroceryItemId is required", nameof(groceryItemId));
        if (price < 0m) throw new ArgumentException("Price must be zero or greater", nameof(price));
        if (discountPercentage.HasValue && (discountPercentage.Value < 0m || discountPercentage.Value > 100m)) throw new ArgumentException("DiscountPercentage must be between 0 and 100", nameof(discountPercentage));
        if (minimumQuantity.HasValue && minimumQuantity.Value < 0m) throw new ArgumentException("MinimumQuantity must be zero or greater", nameof(minimumQuantity));
        if (maximumQuantity.HasValue && maximumQuantity.Value < 0m) throw new ArgumentException("MaximumQuantity must be zero or greater", nameof(maximumQuantity));
        if (minimumQuantity.HasValue && maximumQuantity.HasValue && minimumQuantity.Value > maximumQuantity.Value) throw new ArgumentException("MinimumQuantity cannot be greater than MaximumQuantity", nameof(minimumQuantity));

        Id = id;
        PriceListId = priceListId;
        GroceryItemId = groceryItemId;
        Price = price;
        DiscountPercentage = discountPercentage;
        MinimumQuantity = minimumQuantity;
        MaximumQuantity = maximumQuantity;
        IsActive = true;

        QueueDomainEvent(new PriceListItemCreated { PriceListItem = this });
    }

    public static PriceListItem Create(
        DefaultIdType priceListId,
        DefaultIdType groceryItemId,
        decimal price,
        decimal? discountPercentage = null,
        decimal? minimumQuantity = null,
        decimal? maximumQuantity = null)
    {
        return new PriceListItem(
            DefaultIdType.NewGuid(),
            priceListId,
            groceryItemId,
            price,
            discountPercentage,
            minimumQuantity,
            maximumQuantity);
    }

    public PriceListItem UpdatePrice(decimal price, decimal? discountPercentage = null)
    {
        Price = price;
        DiscountPercentage = discountPercentage;
        QueueDomainEvent(new PriceListItemUpdated { PriceListItem = this });
        return this;
    }

    public decimal GetEffectivePrice()
    {
        return DiscountPercentage.HasValue 
            ? Price * (1 - DiscountPercentage.Value / 100)
            : Price;
    }

    
}
