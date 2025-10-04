namespace FSH.Starter.WebApi.Store.Application.ItemSuppliers.Update.v1;

/// <summary>
/// Command to update an item-supplier relationship.
/// </summary>
public sealed record UpdateItemSupplierCommand(
    DefaultIdType Id,
    // New: keys echoed back by UI during edit; not used to re-key the relationship
    DefaultIdType ItemId,
    DefaultIdType SupplierId,
    // Monetary fields
    decimal? UnitCost,
    // Lead time & quantities
    int? LeadTimeDays,
    int? MinimumOrderQuantity,
    string? SupplierPartNumber,
    int? PackagingQuantity,
    // Flags & ratings
    bool? IsPreferred,
    bool? IsActive,
    decimal? ReliabilityRating,
    // Base fields
    string? Description,
    string? Notes
) : IRequest<UpdateItemSupplierResponse>;
