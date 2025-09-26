using FSH.Starter.Blazor.Client.Services;

namespace FSH.Starter.Blazor.Client.Pages.Store;

/// <summary>
/// Grocery Items page logic. Provides CRUD and search over GroceryItem entities using the generated API client.
/// Mirrors the structure of Budgets and Categories pages for consistency.
/// </summary>
public partial class GroceryItems
{
    [Inject] protected IClient ApiClient { get; set; } = default!;
    [Inject] protected ImageUrlService ImageUrlService { get; set; } = default!;

    private EntityServerTableContext<GroceryItemResponse, DefaultIdType, GroceryItemViewModel> Context { get; set; } = default!;
    private EntityTable<GroceryItemResponse, DefaultIdType, GroceryItemViewModel> _table = default!;

    protected override void OnInitialized()
    {
        Context = new EntityServerTableContext<GroceryItemResponse, DefaultIdType, GroceryItemViewModel>(
            entityName: "Grocery Item",
            entityNamePlural: "Grocery Items",
            entityResource: FshResources.Store,
            fields:
            [
                new EntityField<GroceryItemResponse>(x => x.Sku, "SKU", "SKU"),
                new EntityField<GroceryItemResponse>(x => x.Name, "Name", "Name"),
                new EntityField<GroceryItemResponse>(x => x.Barcode, "Barcode", "Barcode"),
                new EntityField<GroceryItemResponse>(x => x.Price, "Price", "Price", typeof(decimal)),
                new EntityField<GroceryItemResponse>(x => x.Cost, "Cost", "Cost", typeof(decimal)),
                new EntityField<GroceryItemResponse>(x => x.CurrentStock, "Current", "CurrentStock", typeof(int)),
                new EntityField<GroceryItemResponse>(x => x.ReorderPoint, "Reorder", "ReorderPoint", typeof(int)),
                new EntityField<GroceryItemResponse>(x => x.IsPerishable, "Perishable", "IsPerishable", typeof(bool)),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id ?? DefaultIdType.Empty,
            getDetailsFunc: async id =>
            {
                var dto = await ApiClient.GetGroceryItemEndpointAsync("1", id).ConfigureAwait(false);
                return dto.Adapt<GroceryItemViewModel>();
            },
            searchFunc: async filter =>
            {
                var paginationFilter = filter.Adapt<PaginationFilter>();
                var command = paginationFilter.Adapt<SearchGroceryItemsQuery>();
                var result = await ApiClient.SearchGroceryItemsEndpointAsync("1", command).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<GroceryItemResponse>>();
            },
            createFunc: async viewModel =>
            {
                await ApiClient.CreateGroceryItemEndpointAsync("1", viewModel.Adapt<CreateGroceryItemCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, viewModel) =>
            {
                await ApiClient.UpdateGroceryItemEndpointAsync("1", id, viewModel.Adapt<UpdateGroceryItemCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id => await ApiClient.DeleteGroceryItemEndpointAsync("1", id).ConfigureAwait(false),
            importAction: FshActions.Import,
            exportAction: FshActions.Export,
            importFunc: async fileUpload =>
            {
                var command = new ImportGroceryItemsCommand { File = fileUpload };
                return await ApiClient.ImportGroceryItemsEndpointAsync("1", command);
            },
            exportFunc: async filter =>
            {
                var request = filter.Adapt<ExportGroceryItemsQuery>();
                var apiResponse = await ApiClient.ExportGroceryItemsEndpointAsync("1", request);
                return new Components.EntityTable.FileResponse(apiResponse.Stream);
            });
    }
}

/// <summary>
/// ViewModel used by the Grocery Items page for add/edit operations; maps to UpdateGroceryItemCommand for update and to CreateGroceryItemCommand for create.
/// Includes inventory, pricing, and related entity identifiers.
/// </summary>
public class GroceryItemViewModel
{
    /// <summary>Unique identifier of the grocery item.</summary>
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
    public decimal Price { get; set; }

    /// <summary>Supplier cost per unit.</summary>
    public decimal Cost { get; set; }

    /// <summary>Minimum safety stock level.</summary>
    public int MinimumStock { get; set; }

    /// <summary>Maximum stock capacity.</summary>
    public int MaximumStock { get; set; }

    /// <summary>Current available stock.</summary>
    public int CurrentStock { get; set; }

    /// <summary>Threshold to trigger reordering.</summary>
    public int ReorderPoint { get; set; }

    /// <summary>Indicates if the item is perishable.</summary>
    public bool IsPerishable { get; set; }

    /// <summary>Expiry date if perishable.</summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>Optional brand name.</summary>
    public string? Brand { get; set; }

    /// <summary>Optional manufacturer name.</summary>
    public string? Manufacturer { get; set; }

    /// <summary>Item weight.</summary>
    public decimal Weight { get; set; }

    /// <summary>Unit of weight (e.g., kg, lbs).</summary>
    public string? WeightUnit { get; set; }

    /// <summary>Related category identifier.</summary>
    public DefaultIdType? CategoryId { get; set; }

    /// <summary>Related supplier identifier.</summary>
    public DefaultIdType? SupplierId { get; set; }

    /// <summary>Related warehouse location identifier.</summary>
    public DefaultIdType? WarehouseLocationId { get; set; }
    public string? ImageUrl { get; set; }
    public FileUploadCommand? Image { get; set; }
}
