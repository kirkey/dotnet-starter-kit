using System.ComponentModel;
using MediatR;

namespace FSH.Starter.WebApi.Warehouse.Features.Products.Create.v1;

public sealed record CreateProductCommand(
    [property: DefaultValue("Coke")] string Name,
    [property: DefaultValue("SKU-001")] string SKU,
    [property: DefaultValue("1234567890123")] string Barcode,
    [property: DefaultValue("Coca-Cola")] string Brand,
    decimal CostPrice,
    decimal SellingPrice,
    decimal Weight,
    [property: DefaultValue("pcs")] string Unit,
    int ReorderLevel,
    int MaxStockLevel,
    bool IsPerishable,
    int ShelfLifeDays,
    bool RequiresBatchTracking,
    bool IsActive,
    DefaultIdType CategoryId) : IRequest<CreateProductResponse>;

