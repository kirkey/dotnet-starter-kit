using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using Store.Domain.Events;

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
