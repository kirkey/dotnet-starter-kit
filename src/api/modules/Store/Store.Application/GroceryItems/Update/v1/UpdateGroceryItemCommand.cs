namespace FSH.Starter.WebApi.Store.Application.GroceryItems.Update.v1;

public sealed record UpdateGroceryItemCommand(
    DefaultIdType Id,
    string? Name,
    string? Description = null,
    string? Sku = null,
    string? Barcode = null,
    decimal Price = 0,
    decimal Cost = 0,
    int MinimumStock = 0,
    int MaximumStock = 0,
    int CurrentStock = 0,
    int ReorderPoint = 0,
    bool IsPerishable = false,
    DateTime? ExpiryDate = null,
    string? Brand = null,
    string? Manufacturer = null,
    decimal Weight = 0,
    string? WeightUnit = null,
    DefaultIdType? CategoryId = null,
    DefaultIdType? SupplierId = null,
    DefaultIdType? WarehouseLocationId = null) : IRequest<UpdateGroceryItemResponse>;
