namespace FSH.Starter.WebApi.Store.Application.Items.Update.v1;

/// <summary>
/// Command to update an existing Item entity.
/// Includes all inventory, pricing, tracking, and dimension properties.
/// </summary>
public sealed record UpdateItemCommand(
    DefaultIdType Id,
    string? Name,
    string? Description,
    string? Sku,
    string? Barcode,
    decimal? UnitPrice,
    decimal? Cost,
    int? MinimumStock,
    int? MaximumStock,
    int? ReorderPoint,
    int? ReorderQuantity,
    int? LeadTimeDays,
    DefaultIdType? CategoryId,
    DefaultIdType? SupplierId,
    string? UnitOfMeasure,
    bool? IsPerishable,
    bool? IsSerialTracked,
    bool? IsLotTracked,
    int? ShelfLifeDays,
    string? Brand,
    string? Manufacturer,
    string? ManufacturerPartNumber,
    decimal? Weight,
    string? WeightUnit,
    decimal? Length,
    decimal? Width,
    decimal? Height,
    string? DimensionUnit
) : IRequest<UpdateItemResponse>;
