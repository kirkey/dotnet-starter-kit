namespace Store.Domain;

/// <summary>
/// Represents volume-based pricing tiers for wholesale contracts with specific grocery items and quantity ranges.
/// </summary>
/// <remarks>
/// Use cases:
/// - Define tiered pricing structures for wholesale customers based on order quantities.
/// - Support volume discounts and bulk pricing incentives for large orders.
/// - Enable contract-specific pricing that overrides standard wholesale rates.
/// - Manage time-limited promotional pricing for wholesale customers.
/// - Support quantity break pricing with minimum and maximum quantity thresholds.
/// - Track pricing effectiveness and volume commitment compliance.
/// - Enable competitive pricing strategies for key wholesale accounts.
/// - Support seasonal or event-based wholesale pricing adjustments.
/// 
/// Default values:
/// - WholesaleContractId: required parent contract reference
/// - GroceryItemId: required item reference for pricing
/// - MinimumQuantity: required minimum order quantity (example: 100 units)
/// - MaximumQuantity: optional maximum quantity for tier (example: 999 units)
/// - UnitPrice: required wholesale price per unit (example: 2.50)
/// - DiscountPercentage: optional additional discount (example: 5.0 for 5% off)
/// - EffectiveDate: required pricing start date (example: 2025-10-01)
/// - ExpiryDate: optional pricing end date (example: 2025-12-31)
/// - IsActive: true (pricing is active by default)
/// - Currency: "USD" (default currency unless specified)
/// 
/// Business rules:
/// - MinimumQuantity must be positive and less than MaximumQuantity
/// - UnitPrice must be positive
/// - DiscountPercentage must be between 0 and 100
/// - EffectiveDate cannot be in the past for new pricing
/// - ExpiryDate must be after EffectiveDate when specified
/// - Cannot have overlapping quantity ranges for same item and contract
/// - Contract must be active to create new pricing tiers
/// - Pricing tiers should not conflict with existing active pricing
/// - Volume discounts should be progressive (higher quantities = better pricing)
/// </remarks>
/// <seealso cref="Store.Domain.Events.WholesalePricingCreated"/>
/// <seealso cref="Store.Domain.Events.WholesalePricingUpdated"/>
/// <seealso cref="Store.Domain.Events.WholesalePricingActivated"/>
/// <seealso cref="Store.Domain.Events.WholesalePricingExpired"/>
/// <seealso cref="Store.Domain.Exceptions.WholesalePricing.WholesalePricingNotFoundException"/>
/// <seealso cref="Store.Domain.Exceptions.WholesalePricing.InvalidWholesalePricingRangeException"/>
public sealed class WholesalePricing : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Parent wholesale contract id.
    /// </summary>
    public DefaultIdType WholesaleContractId { get; private set; }

    /// <summary>
    /// Grocery item id this pricing applies to.
    /// </summary>
    public DefaultIdType GroceryItemId { get; private set; }

    /// <summary>
    /// Minimum quantity required for this tier.
    /// </summary>
    public int MinimumQuantity { get; private set; }

    /// <summary>
    /// Optional maximum quantity for the tier.
    /// </summary>
    public int? MaximumQuantity { get; private set; }

    /// <summary>
    /// Price to charge for items in this tier.
    /// </summary>
    public decimal TierPrice { get; private set; }

    /// <summary>
    /// Discount percentage applied at this tier (0-100).
    /// </summary>
    public decimal DiscountPercentage { get; private set; }

    /// <summary>
    /// Effective date for this pricing tier.
    /// </summary>
    public DateTime EffectiveDate { get; private set; }

    /// <summary>
    /// Optional expiry date after which this pricing is no longer valid.
    /// </summary>
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
        // validations
        if (wholesaleContractId == default) throw new ArgumentException("WholesaleContractId is required", nameof(wholesaleContractId));
        if (groceryItemId == default) throw new ArgumentException("GroceryItemId is required", nameof(groceryItemId));
        if (minimumQuantity <= 0) throw new ArgumentException("MinimumQuantity must be greater than zero", nameof(minimumQuantity));
        if (maximumQuantity.HasValue && maximumQuantity.Value < minimumQuantity) throw new ArgumentException("MaximumQuantity must be greater than or equal to MinimumQuantity", nameof(maximumQuantity));
        if (tierPrice < 0m) throw new ArgumentException("TierPrice must be zero or greater", nameof(tierPrice));
        if (discountPercentage is < 0m or > 100m) throw new ArgumentException("DiscountPercentage must be between 0 and 100", nameof(discountPercentage));
        if (effectiveDate == default) throw new ArgumentException("EffectiveDate is required", nameof(effectiveDate));
        if (expiryDate.HasValue && expiryDate.Value < effectiveDate) throw new ArgumentException("ExpiryDate cannot be earlier than EffectiveDate", nameof(expiryDate));

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
