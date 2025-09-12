namespace FSH.Starter.WebApi.Store.Application.GroceryItems.Get.v1;

public sealed record GroceryItemResponse(
    DefaultIdType? Id, 
    string Name, 
    string? Description, 
    string SKU,
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
    string? WeightUnit);
