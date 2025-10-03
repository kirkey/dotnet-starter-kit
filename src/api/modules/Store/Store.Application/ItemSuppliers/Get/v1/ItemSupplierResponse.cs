namespace FSH.Starter.WebApi.Store.Application.ItemSuppliers.Get.v1;

/// <summary>
/// Response containing item-supplier relationship details.
/// </summary>
public sealed record ItemSupplierResponse(
    DefaultIdType Id,
    DefaultIdType ItemId,
    DefaultIdType SupplierId,
    string? SupplierPartNumber,
    decimal UnitCost,
    int LeadTimeDays,
    int MinimumOrderQuantity,
    int? PackagingQuantity,
    string CurrencyCode,
    bool IsPreferred,
    bool IsActive,
    decimal? ReliabilityRating,
    DateTime? LastPriceUpdate,
    DateTimeOffset CreatedOn,
    DefaultIdType CreatedBy
);
