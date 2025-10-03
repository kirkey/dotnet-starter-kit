namespace FSH.Starter.WebApi.Store.Application.Items.Update.v1;

public sealed record UpdateItemCommand(
    DefaultIdType Id,
    string? Name,
    string? Description,
    string? Sku,
    string? Barcode,
    decimal? UnitPrice,
    decimal? Cost,
    DefaultIdType? CategoryId,
    DefaultIdType? SupplierId,
    string? Brand,
    string? Manufacturer,
    string? ManufacturerPartNumber,
    string? UnitOfMeasure
) : IRequest<UpdateItemResponse>;
