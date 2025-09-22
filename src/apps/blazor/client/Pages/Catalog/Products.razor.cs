namespace FSH.Starter.Blazor.Client.Pages.Catalog;

public partial class Products
{
    [Inject] protected IClient _client { get; set; } = default!;

    protected EntityServerTableContext<ProductResponse, DefaultIdType, ProductViewModel> Context { get; set; } = default!;

    private EntityTable<ProductResponse, DefaultIdType, ProductViewModel> _table = default!;

    private List<BrandResponse> _brands = [];

    protected override async Task OnInitializedAsync()
    {
        Context = new EntityServerTableContext<ProductResponse, DefaultIdType, ProductViewModel>(
            entityName: "Product",
            entityNamePlural: "Products",
            entityResource: FshResources.Products,
            fields:
            [
                new EntityField<ProductResponse>(response => response.Id, "Id", "Id"),
                new EntityField<ProductResponse>(response => response.Name, "Name", "Name"),
                new EntityField<ProductResponse>(response => response.Description, "Description", "Description"),
                new EntityField<ProductResponse>(response => response.Price, "Price", "Price"),
                new EntityField<ProductResponse>(response => response.Brand?.Name, "Brand", "Brand")
            ],
            enableAdvancedSearch: true,
            idFunc: response => response.Id!.Value,
            searchFunc: async filter =>
            {
                var productFilter = filter.Adapt<SearchProductsCommand>();
                productFilter.MinimumRate = Convert.ToDouble(SearchMinimumRate);
                productFilter.MaximumRate = Convert.ToDouble(SearchMaximumRate);
                productFilter.BrandId = SearchBrandId;
                var result = await _client.SearchProductsEndpointAsync("1", productFilter);
                return result.Adapt<PaginationResponse<ProductResponse>>();
            },
            createFunc: async viewModel =>
            {
                await _client.CreateProductEndpointAsync("1", viewModel.Adapt<CreateProductCommand>());
            },
            updateFunc: async (id, viewModel) =>
            {
                await _client.UpdateProductEndpointAsync("1", id, viewModel.Adapt<UpdateProductCommand>());
            },
            deleteFunc: async id => await _client.DeleteProductEndpointAsync("1", id));

        await LoadBrandsAsync();
    }

    private async Task LoadBrandsAsync()
    {
        if (_brands.Count == 0)
        {
            var response = await _client.SearchBrandsEndpointAsync("1", new SearchBrandsCommand());
            if (response?.Items != null)
            {
                _brands = [.. response.Items];
            }
        }
    }

    // Advanced Search

    private DefaultIdType? _searchBrandId;
    private DefaultIdType? SearchBrandId
    {
        get => _searchBrandId;
        set
        {
            _searchBrandId = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private decimal _searchMinimumRate;
    private decimal SearchMinimumRate
    {
        get => _searchMinimumRate;
        set
        {
            _searchMinimumRate = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private decimal _searchMaximumRate = 9999;
    private decimal SearchMaximumRate
    {
        get => _searchMaximumRate;
        set
        {
            _searchMaximumRate = value;
            _ = _table.ReloadDataAsync();
        }
    }
}

public partial class ProductViewModel : UpdateProductCommand;
