using FSH.Starter.Blazor.Client.Services;

namespace FSH.Starter.Blazor.Client.Pages.Store.Items;

/// <summary>
/// Items page logic. Provides CRUD and search over Item entities using the generated API client.
/// Mirrors the structure of Budgets and Categories pages for consistency.
/// </summary>
public partial class Items
{
    [Inject] protected ImageUrlService ImageUrlService { get; set; } = default!;

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
                new EntityField<ItemResponse>(response => response.ImageUrl, "Image", "ImageUrl", Template: TemplateImage),
                new EntityField<ItemResponse>(response => response.Sku, "SKU", "SKU"),
                new EntityField<ItemResponse>(response => response.Barcode, "Barcode", "Barcode"),
                new EntityField<ItemResponse>(response => response.Name, "Name", "Name"),
                new EntityField<ItemResponse>(response => response.Brand, "Brand", "Brand"),
                new EntityField<ItemResponse>(response => response.UnitPrice, "Price", "UnitPrice", typeof(decimal)),
                new EntityField<ItemResponse>(response => response.Cost, "Cost", "Cost", typeof(decimal)),
                new EntityField<ItemResponse>(response => response.MinimumStock, "Min Stock", "MinimumStock", typeof(int)),
                new EntityField<ItemResponse>(response => response.ReorderPoint, "Reorder", "ReorderPoint", typeof(int)),
                new EntityField<ItemResponse>(response => response.IsPerishable, "Perishable", "IsPerishable", typeof(bool)),
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
                viewModel.Image = new FileUploadCommand
                {
                    Name = viewModel.Image?.Name,
                    Extension = viewModel.Image?.Extension,
                    Data = viewModel.Image?.Data,
                    Size = viewModel.Image?.Size,
                };
                await Client.CreateItemEndpointAsync("1", viewModel.Adapt<CreateItemCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, viewModel) =>
            {
                viewModel.Image = new FileUploadCommand
                {
                    Name = viewModel.Image?.Name,
                    Extension = viewModel.Image?.Extension,
                    Data = viewModel.Image?.Data,
                    Size = viewModel.Image?.Size,
                };
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

