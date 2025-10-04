namespace FSH.Starter.WebApi.Store.Application.ItemSuppliers.Get.v1;

/// <summary>
/// Response containing item-supplier relationship details.
/// </summary>
public sealed record ItemSupplierResponse(
    DefaultIdType Id,
    string Name,
    string? Description,
    string? Notes,
    DefaultIdType ItemId,
    DefaultIdType SupplierId,
    string? SupplierPartNumber,
    decimal UnitCost,
    int LeadTimeDays,
    int MinimumOrderQuantity,
    int? PackagingQuantity,
    bool IsPreferred,
    bool IsActive,
    decimal? ReliabilityRating,
    DateTime? LastPriceUpdate,
    DateTime CreatedOn,
    DateTime? LastModifiedOn
);
