namespace FSH.Starter.WebApi.Store.Application.WholesalePricings.Create.v1;

public sealed record CreateWholesalePricingCommand(
    DefaultIdType WholesaleContractId,
    DefaultIdType GroceryItemId,
    int MinimumQuantity,
    int? MaximumQuantity,
    decimal TierPrice,
    decimal DiscountPercentage,
    DateTime EffectiveDate,
    DateTime? ExpiryDate = null,
    [property: DefaultValue(null)] string? Notes = null
) : IRequest<CreateWholesalePricingResponse>;

