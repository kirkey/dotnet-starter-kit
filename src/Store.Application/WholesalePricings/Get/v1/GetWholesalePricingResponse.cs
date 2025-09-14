namespace FSH.Starter.WebApi.Store.Application.WholesalePricings.Get.v1;

public sealed record GetWholesalePricingResponse(
    DefaultIdType Id,
    DefaultIdType WholesaleContractId,
    DefaultIdType GroceryItemId,
    int MinimumQuantity,
    int? MaximumQuantity,
    decimal TierPrice,
    decimal DiscountPercentage,
    DateTime EffectiveDate,
    DateTime? ExpiryDate,
    bool IsActive,
    string? Notes,
    DateTimeOffset CreatedOn,
    DateTimeOffset LastModifiedOn);

