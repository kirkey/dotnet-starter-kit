namespace FSH.Starter.WebApi.Store.Application.ItemSuppliers.Update.v1;

/// <summary>
/// Command to update an item-supplier relationship.
/// </summary>
public sealed record UpdateItemSupplierCommand(
    DefaultIdType Id,
    decimal? UnitCost,
    int? LeadTimeDays,
    int? MinimumOrderQuantity,
    string? SupplierPartNumber,
    int? PackagingQuantity,
    bool? IsPreferred,
    bool? IsActive,
    decimal? ReliabilityRating,
    string? Description,
    string? Notes
) : IRequest<UpdateItemSupplierResponse>;
