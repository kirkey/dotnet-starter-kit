namespace FSH.Starter.WebApi.Store.Application.PriceLists.Search.v1;

public record GetPriceListListResponse(
    DefaultIdType Id,
    string Name,
    string PriceListName,
    DateTime EffectiveDate,
    DateTime? ExpiryDate,
    bool IsActive,
    string Currency);

