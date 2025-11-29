namespace FSH.Starter.Blazor.Client.Pages.Store.Items;

/// <summary>
/// Items page logic. Provides CRUD and search over Item entities using the generated API client.
/// Mirrors the structure of Budgets and Categories pages for consistency.
/// Supports advanced search filtering by category, supplier, perishability, and price range.
/// </summary>
public partial class Items
{
    [Inject] protected ImageUrlService ImageUrlService { get; set; } = null!;
    [Inject] protected ICourier Courier { get; set; } = null!;

    protected EntityServerTableContext<ItemResponse, DefaultIdType, ItemViewModel> Context { get; set; } = null!;
    private EntityTable<ItemResponse, DefaultIdType, ItemViewModel> _table = null!;

    // Advanced search filters
    private DefaultIdType? _searchCategoryId;
    private DefaultIdType? SearchCategoryId
    {
        get => _searchCategoryId;
        set
        {
            _searchCategoryId = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private DefaultIdType? _searchSupplierId;
    private DefaultIdType? SearchSupplierId
    {
        get => _searchSupplierId;
        set
        {
            _searchSupplierId = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private bool? _searchIsPerishable;
    private bool? SearchIsPerishable
    {
        get => _searchIsPerishable;
        set
        {
            _searchIsPerishable = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private decimal? _searchMinPrice;
    private decimal? SearchMinPrice
    {
        get => _searchMinPrice;
        set
        {
            _searchMinPrice = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private decimal? _searchMaxPrice;
    private decimal? SearchMaxPrice
    {
        get => _searchMaxPrice;
        set
        {
            _searchMaxPrice = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private ClientPreference _preference = new();

    protected override async Task OnInitializedAsync()
    {
        // Load preference
        if (await ClientPreferences.GetPreference() is ClientPreference preference)
        {
            _preference = preference;
        }

        // Subscribe to preference changes
        Courier.SubscribeWeak<NotificationWrapper<ClientPreference>>(wrapper =>
        {
            _preference.Elevation = ClientPreference.SetClientPreference(wrapper.Notification);
            _preference.BorderRadius = ClientPreference.SetClientBorderRadius(wrapper.Notification);
            StateHasChanged();
            return Task.CompletedTask;
        });

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
            searchFunc: async filter =>
            {
                var command = new SearchItemsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy,
                    CategoryId = SearchCategoryId,
                    SupplierId = SearchSupplierId,
                    IsPerishable = SearchIsPerishable,
                    MinPrice = SearchMinPrice,
                    MaxPrice = SearchMaxPrice
                };
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
                        CategoryId = SearchCategoryId,
                        SupplierId = SearchSupplierId,
                        IsPerishable = SearchIsPerishable,
                        MinPrice = SearchMinPrice,
                        MaxPrice = SearchMaxPrice
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

    /// <summary>
    /// Show items help dialog.
    /// </summary>
    private async Task ShowItemsHelp()
    {
        await DialogService.ShowAsync<ItemsHelpDialog>("Items Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }
}

