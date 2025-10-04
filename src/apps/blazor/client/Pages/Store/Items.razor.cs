namespace FSH.Starter.Blazor.Client.Pages.Store;

/// <summary>
/// Items page logic. Provides CRUD and search over Item entities using the generated API client.
/// Mirrors the structure of Budgets and Categories pages for consistency.
/// </summary>
public partial class Items
{
    protected EntityServerTableContext<ItemResponse, DefaultIdType, ItemViewModel> Context { get; set; } = default!;
    private EntityTable<ItemResponse, DefaultIdType, ItemViewModel> _table = default!;

    protected override async Task OnInitializedAsync()
    {
        Context = new EntityServerTableContext<ItemResponse, DefaultIdType, ItemViewModel>(
            entityName: "Item",
            entityNamePlural: "Items",
            entityResource: FshResources.Store,
            fields:
            [
                new EntityField<ItemResponse>(x => x.Sku, "SKU", "SKU"),
                new EntityField<ItemResponse>(x => x.Barcode, "Barcode", "Barcode"),
                new EntityField<ItemResponse>(x => x.Name, "Name", "Name"),
                new EntityField<ItemResponse>(x => x.Brand, "Brand", "Brand"),
                new EntityField<ItemResponse>(x => x.UnitPrice, "Price", "UnitPrice", typeof(double)),
                new EntityField<ItemResponse>(x => x.Cost, "Cost", "Cost", typeof(double)),
                new EntityField<ItemResponse>(x => x.MinimumStock, "Min Stock", "MinimumStock", typeof(int)),
                new EntityField<ItemResponse>(x => x.ReorderPoint, "Reorder", "ReorderPoint", typeof(int)),
                new EntityField<ItemResponse>(x => x.IsPerishable, "Perishable", "IsPerishable", typeof(bool)),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            getDetailsFunc: async id =>
            {
                var dto = await Client.GetItemEndpointAsync("1", id).ConfigureAwait(false);
                return dto.Adapt<ItemViewModel>();
            },
            searchFunc: async filter =>
            {
                var paginationFilter = filter.Adapt<PaginationFilter>();
                var command = paginationFilter.Adapt<SearchItemsCommand>();
                var result = await Client.SearchItemsEndpointAsync("1", command).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<ItemResponse>>();
            },
            createFunc: async viewModel =>
            {
                await Client.CreateItemEndpointAsync("1", viewModel.Adapt<CreateItemCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, viewModel) =>
            {
                await Client.UpdateItemEndpointAsync("1", id, viewModel.Adapt<UpdateItemCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.DeleteItemEndpointAsync("1", id).ConfigureAwait(false));
        
        await Task.CompletedTask;
    }
}

/// <summary>
/// ViewModel used by the Items page for add/edit operations; maps to UpdateItemCommand for update and to CreateItemCommand for create.
/// Includes inventory, pricing, and related entity identifiers.
/// </summary>
public class ItemViewModel
{
    /// <summary>Unique identifier of the item.</summary>
    public DefaultIdType Id { get; set; }

    /// <summary>Display name of the item.</summary>
    public string? Name { get; set; }

    /// <summary>Optional detailed description.</summary>
    public string? Description { get; set; }

    /// <summary>Stock keeping unit (uppercase letters and digits).</summary>
    public string? Sku { get; set; }

    /// <summary>Barcode value (uppercase letters and digits).</summary>
    public string? Barcode { get; set; }

    /// <summary>Unit price.</summary>
    public double UnitPrice { get; set; }

    /// <summary>Supplier cost per unit.</summary>
    public double Cost { get; set; }

    /// <summary>Minimum safety stock level.</summary>
    public int MinimumStock { get; set; }

    /// <summary>Maximum stock capacity.</summary>
    public int MaximumStock { get; set; }

    /// <summary>Threshold to trigger reordering.</summary>
    public int ReorderPoint { get; set; }

    /// <summary>Indicates if the item is perishable.</summary>
    public bool IsPerishable { get; set; }

    /// <summary>Shelf life in days if perishable.</summary>
    public int? ShelfLifeDays { get; set; }

    /// <summary>Is serial number tracked.</summary>
    public bool IsSerialTracked { get; set; }

    /// <summary>Is lot number tracked.</summary>
    public bool IsLotTracked { get; set; }

    /// <summary>Optional brand name.</summary>
    public string? Brand { get; set; }

    /// <summary>Optional manufacturer name.</summary>
    public string? Manufacturer { get; set; }

    /// <summary>Item weight.</summary>
    public double Weight { get; set; }

    /// <summary>Lead time in days.</summary>
    public int LeadTimeDays { get; set; }

    /// <summary>Reorder quantity.</summary>
    public int ReorderQuantity { get; set; }

    /// <summary>Unit of weight (e.g., kg, lbs).</summary>
    public string? WeightUnit { get; set; }

    /// <summary>Related category identifier.</summary>
    public DefaultIdType? CategoryId { get; set; }

    /// <summary>Related supplier identifier.</summary>
    public DefaultIdType? SupplierId { get; set; }

    /// <summary>Unit of measure.</summary>
    public string? UnitOfMeasure { get; set; }

    /// <summary>Manufacturer part number.</summary>
    public string? ManufacturerPartNumber { get; set; }

    /// <summary>Length dimension.</summary>
    public double? Length { get; set; }

    /// <summary>Width dimension.</summary>
    public double? Width { get; set; }

    /// <summary>Height dimension.</summary>
    public double? Height { get; set; }

    /// <summary>Dimension unit (e.g., cm, in).</summary>
    public string? DimensionUnit { get; set; }
}
