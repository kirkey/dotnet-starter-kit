namespace FSH.Starter.WebApi.Store.Application.PriceLists.Get.v1;

public sealed record GetPriceListResponse(
    DefaultIdType Id,
    string Name,
    string? Description,
    string PriceListName,
    string PriceListType,
    DateTime EffectiveDate,
    DateTime? ExpiryDate,
    bool IsActive,
    string Currency,
    decimal? MinimumOrderValue,
    string? CustomerType,
    string? Notes,
    DateTimeOffset CreatedOn,
    DateTimeOffset LastModifiedOn);

