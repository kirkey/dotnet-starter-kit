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
                new EntityField<ItemResponse>(x => x.UnitPrice, "Price", "UnitPrice", typeof(decimal)),
                new EntityField<ItemResponse>(x => x.Cost, "Cost", "Cost", typeof(decimal)),
                new EntityField<ItemResponse>(x => x.MinimumStock, "Min Stock", "MinimumStock", typeof(int)),
                new EntityField<ItemResponse>(x => x.ReorderPoint, "Reorder", "ReorderPoint", typeof(int)),
                new EntityField<ItemResponse>(x => x.IsPerishable, "Perishable", "IsPerishable", typeof(bool)),
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id,
            // getDetailsFunc: async id =>
            // {
            //     var dto = await Client.GetItemEndpointAsync("1", id).ConfigureAwait(false);
            //     return dto.Adapt<ItemViewModel>();
            // },
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
            deleteFunc: async id => await Client.DeleteItemEndpointAsync("1", id).ConfigureAwait(false),
            exportFunc: async filter =>
            {
                var exportQuery = new ExportItemsQuery
                {
                    Filter = new ItemExportFilter
                    {
                        SearchTerm = filter.Keyword,
                        CategoryId = null, // Can be extended to include category filter from advanced search
                        SupplierId = null, // Can be extended to include supplier filter from advanced search
                        IsPerishable = null,
                        MinPrice = null,
                        MaxPrice = null
                    },
                    SheetName = "Items"
                };
                
                var result = await Client.ExportItemsEndpointAsync("1", exportQuery).ConfigureAwait(false);
                var stream = new MemoryStream(result.Data);
                return new Components.EntityTable.FileResponse(stream);
            },
            importFunc: async fileUpload =>
            {
                var command = new ImportItemsCommand
                {
                    File = fileUpload,
                    SheetName = "Sheet1",
                    ValidateStructure = true
                };
                
                var result = await Client.ImportItemsEndpointAsync("1", command).ConfigureAwait(false);
                return result;
            });
        
        await Task.CompletedTask;
    }
}

/// <summary>
/// ViewModel used by the Items page for add/edit operations.
/// Inherits from UpdateItemCommand to ensure proper mapping with the API.
/// </summary>
public partial class ItemViewModel : UpdateItemCommand;
