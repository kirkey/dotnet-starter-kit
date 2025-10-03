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
    string? CurrencyCode,
    bool? IsPreferred,
    bool? IsActive,
    decimal? ReliabilityRating
) : IRequest<UpdateItemSupplierResponse>;
