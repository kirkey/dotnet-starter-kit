using MediatR;

namespace FSH.Starter.WebApi.Warehouse.Features.Products.Update.v1;

public sealed record UpdateProductCommand(
    DefaultIdType Id,
    string? Name,
    string? SKU,
    string? Barcode,
    string? Brand,
    decimal? CostPrice,
    decimal? SellingPrice,
    decimal? Weight,
    string? Unit,
    int? ReorderLevel,
    int? MaxStockLevel,
    bool? IsPerishable,
    int? ShelfLifeDays,
    bool? RequiresBatchTracking,
    bool? IsActive,
    DefaultIdType? CategoryId) : IRequest<UpdateProductResponse>;

