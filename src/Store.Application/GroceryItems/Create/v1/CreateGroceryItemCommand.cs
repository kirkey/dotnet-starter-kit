namespace FSH.Starter.WebApi.Store.Application.GroceryItems.Create.v1;

public sealed record CreateGroceryItemCommand(
    [property: DefaultValue("Sample Grocery Item")] string? Name,
    [property: DefaultValue("Descriptive Description")] string? Description = null,
    [property: DefaultValue("SKU001")] string? Sku = null,
    [property: DefaultValue("1234567890123")] string? Barcode = null,
    [property: DefaultValue(10.99)] decimal Price = 10.99m,
    [property: DefaultValue(5.99)] decimal Cost = 5.99m,
    [property: DefaultValue(10)] int MinimumStock = 10,
    [property: DefaultValue(100)] int MaximumStock = 100,
    [property: DefaultValue(50)] int CurrentStock = 50,
    [property: DefaultValue(20)] int ReorderPoint = 20,
    [property: DefaultValue(false)] bool IsPerishable = false,
    [property: DefaultValue(null)] DateTime? ExpiryDate = null,
    [property: DefaultValue("Generic Brand")] string? Brand = null,
    [property: DefaultValue("Generic Manufacturer")] string? Manufacturer = null,
    [property: DefaultValue(1.0)] decimal Weight = 1.0m,
    [property: DefaultValue("kg")] string? WeightUnit = "kg",
    [property: DefaultValue(null)] DefaultIdType? CategoryId = null,
    [property: DefaultValue(null)] DefaultIdType? SupplierId = null,
    [property: DefaultValue(null)] DefaultIdType? WarehouseLocationId = null
) : IRequest<CreateGroceryItemResponse>;
