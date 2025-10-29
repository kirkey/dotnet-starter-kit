using FSH.Framework.Core.Storage.File.Features;

namespace FSH.Starter.WebApi.Store.Application.Items.Create.v1;

public sealed record CreateItemCommand(
    [property: DefaultValue("Sample Item")] string? Name,
    [property: DefaultValue("Detailed description")] string? Description = null,
    [property: DefaultValue("ITEM-001")] string? Sku = null,
    [property: DefaultValue("0123456789012")] string? Barcode = null,
    [property: DefaultValue(24.99)] decimal UnitPrice = 24.99m,
    [property: DefaultValue(15.00)] decimal Cost = 15.00m,
    [property: DefaultValue(10)] int MinimumStock = 10,
    [property: DefaultValue(500)] int MaximumStock = 500,
    [property: DefaultValue(25)] int ReorderPoint = 25,
    [property: DefaultValue(100)] int ReorderQuantity = 100,
    [property: DefaultValue(7)] int LeadTimeDays = 7,
    DefaultIdType? CategoryId = null,
    DefaultIdType? SupplierId = null,
    [property: DefaultValue("EA")] string? UnitOfMeasure = "EA",
    [property: DefaultValue(false)] bool IsPerishable = false,
    [property: DefaultValue(false)] bool IsSerialTracked = false,
    [property: DefaultValue(false)] bool IsLotTracked = false,
    [property: DefaultValue(null)] int? ShelfLifeDays = null,
    [property: DefaultValue(null)] string? Brand = null,
    [property: DefaultValue(null)] string? Manufacturer = null,
    [property: DefaultValue(null)] string? ManufacturerPartNumber = null,
    [property: DefaultValue(2.5)] decimal Weight = 2.5m,
    [property: DefaultValue("kg")] string? WeightUnit = "kg",
    [property: DefaultValue(null)] decimal? Length = null,
    [property: DefaultValue(null)] decimal? Width = null,
    [property: DefaultValue(null)] decimal? Height = null,
    [property: DefaultValue(null)] string? DimensionUnit = null,
    [property: DefaultValue(null)] string? Notes = null,
    [property: DefaultValue(null)] string? ImageUrl = null
) : IRequest<CreateItemResponse>
{
    /// <summary>
    /// Optional image payload uploaded by the client. When provided, the image is uploaded to storage and ImageUrl is set from the saved file name.
    /// </summary>
    public FileUploadCommand? Image { get; init; }
}
