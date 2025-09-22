namespace FSH.Starter.WebApi.Store.Application.GroceryItems.Get.v1;

public sealed record GroceryItemResponse(
    DefaultIdType? Id, 
    string Name, 
    string? Description, 
    string Sku,
    string Barcode,
    decimal Price, 
    decimal Cost,
    int MinimumStock,
    int MaximumStock,
    int CurrentStock,
    int ReorderPoint,
    bool IsPerishable,
    DateTime? ExpiryDate,
    string? Brand,
    string? Manufacturer,
    decimal Weight,
    string? WeightUnit,
    // Added related entity identifiers for client-side editing and consistency
    DefaultIdType? CategoryId,
    DefaultIdType? SupplierId,
    DefaultIdType? WarehouseLocationId);
