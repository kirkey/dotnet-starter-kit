namespace FSH.Starter.WebApi.Store.Application.PriceLists.Update.v1;

public sealed record UpdatePriceListCommand(
    DefaultIdType Id,
    [property: DefaultValue("Default Price List")] string Name,
    [property: DefaultValue(null)] string? Description,
    [property: DefaultValue("DEFAULT")] string PriceListName,
    [property: DefaultValue("Retail")] string PriceListType,
    DateTime EffectiveDate,
    DateTime? ExpiryDate,
    [property: DefaultValue(true)] bool IsActive,
    [property: DefaultValue("USD")] string Currency,
    [property: DefaultValue(null)] decimal? MinimumOrderValue,
    [property: DefaultValue(null)] string? CustomerType,
    [property: DefaultValue(null)] string? Notes
) : IRequest<UpdatePriceListResponse>;

