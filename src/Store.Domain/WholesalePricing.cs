using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using Store.Domain.Events;

namespace Store.Domain;

public sealed class WholesalePricing : AuditableEntity, IAggregateRoot
{
    public DefaultIdType WholesaleContractId { get; private set; }
    public DefaultIdType GroceryItemId { get; private set; }
    public int MinimumQuantity { get; private set; }
    public int? MaximumQuantity { get; private set; }
    public decimal TierPrice { get; private set; }
    public decimal DiscountPercentage { get; private set; }
    public DateTime EffectiveDate { get; private set; }
    public DateTime? ExpiryDate { get; private set; }
    public bool IsActive { get; private set; } = true;
    
    
    public WholesaleContract WholesaleContract { get; private set; } = default!;
    public GroceryItem GroceryItem { get; private set; } = default!;

    private WholesalePricing() { }

    private WholesalePricing(
        DefaultIdType id,
        DefaultIdType wholesaleContractId,
        DefaultIdType groceryItemId,
        int minimumQuantity,
        int? maximumQuantity,
        decimal tierPrice,
        decimal discountPercentage,
        DateTime effectiveDate,
        DateTime? expiryDate,
        string? notes)
    {
        Id = id;
        WholesaleContractId = wholesaleContractId;
        GroceryItemId = groceryItemId;
        MinimumQuantity = minimumQuantity;
        MaximumQuantity = maximumQuantity;
        TierPrice = tierPrice;
        DiscountPercentage = discountPercentage;
        EffectiveDate = effectiveDate;
        ExpiryDate = expiryDate;
        IsActive = true;
        Notes = notes;

        QueueDomainEvent(new WholesalePricingCreated { WholesalePricing = this });
    }

    public static WholesalePricing Create(
        DefaultIdType wholesaleContractId,
        DefaultIdType groceryItemId,
        int minimumQuantity,
        int? maximumQuantity,
        decimal tierPrice,
        decimal discountPercentage,
        DateTime effectiveDate,
        DateTime? expiryDate = null,
        string? notes = null)
    {
        return new WholesalePricing(
            DefaultIdType.NewGuid(),
            wholesaleContractId,
            groceryItemId,
            minimumQuantity,
            maximumQuantity,
            tierPrice,
            discountPercentage,
            effectiveDate,
            expiryDate,
            notes);
    }

    public WholesalePricing UpdatePricing(decimal tierPrice, decimal discountPercentage)
    {
        if (TierPrice == tierPrice && DiscountPercentage == discountPercentage)
        {
            return this;
        }

        TierPrice = tierPrice;
        DiscountPercentage = discountPercentage;
        QueueDomainEvent(new WholesalePricingUpdated { WholesalePricing = this });
        return this;
    }

    public WholesalePricing Deactivate()
    {
        if (!IsActive)
        {
            return this;
        }

        IsActive = false;
        QueueDomainEvent(new WholesalePricingDeactivated { WholesalePricing = this });
        return this;
    }

    public bool IsValidForQuantity(int quantity) => 
        quantity >= MinimumQuantity && (!MaximumQuantity.HasValue || quantity <= MaximumQuantity.Value);
    
    public bool IsCurrentlyActive() => 
        IsActive && DateTime.UtcNow >= EffectiveDate && (!ExpiryDate.HasValue || DateTime.UtcNow <= ExpiryDate.Value);
}
