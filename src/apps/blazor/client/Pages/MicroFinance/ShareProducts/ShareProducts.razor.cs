namespace FSH.Starter.Blazor.Client.Pages.MicroFinance.ShareProducts;

/// <summary>
/// ShareProducts page logic. Provides CRUD and search over ShareProduct entities.
/// Manages share product configurations for member ownership.
/// </summary>
public partial class ShareProducts
{
    protected EntityServerTableContext<ShareProductResponse, DefaultIdType, ShareProductViewModel> Context { get; set; } = null!;

    private EntityTable<ShareProductResponse, DefaultIdType, ShareProductViewModel> _table = null!;

    [CascadingParameter]
    protected Task<AuthenticationState> AuthState { get; set; } = null!;

    [Inject]
    protected IAuthorizationService AuthService { get; set; } = null!;

    private ClientPreference _preference = new();

    // Advanced search filters
    private string? _searchCode;
    private string? SearchCode
    {
        get => _searchCode;
        set
        {
            _searchCode = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private string? _searchName;
    private string? SearchName
    {
        get => _searchName;
        set
        {
            _searchName = value;
            _ = _table.ReloadDataAsync();
        }
    }

    private bool? _searchPaysDividends;
    private bool? SearchPaysDividends
    {
        get => _searchPaysDividends;
        set
        {
            _searchPaysDividends = value;
            _ = _table.ReloadDataAsync();
        }
    }

    protected override async Task OnInitializedAsync()
    {
        if (await ClientPreferences.GetPreference() is ClientPreference preference)
        {
            _preference = preference;
        }

        Courier.SubscribeWeak<NotificationWrapper<ClientPreference>>(wrapper =>
        {
            _preference.Elevation = ClientPreference.SetClientPreference(wrapper.Notification);
            _preference.BorderRadius = ClientPreference.SetClientBorderRadius(wrapper.Notification);
            StateHasChanged();
            return Task.CompletedTask;
        });

        Context = new EntityServerTableContext<ShareProductResponse, DefaultIdType, ShareProductViewModel>(
            fields:
            [
                new EntityField<ShareProductResponse>(dto => dto.Code, "Code", "Code"),
                new EntityField<ShareProductResponse>(dto => dto.Name, "Name", "Name"),
                new EntityField<ShareProductResponse>(dto => dto.NominalValue, "Nominal Value", "NominalValue", typeof(decimal)),
                new EntityField<ShareProductResponse>(dto => dto.CurrentPrice, "Current Price", "CurrentPrice", typeof(decimal)),
                new EntityField<ShareProductResponse>(dto => dto.MinSharesForMembership, "Min Shares", "MinSharesForMembership", typeof(int)),
                new EntityField<ShareProductResponse>(dto => dto.PaysDividends, "Dividends", "PaysDividends", typeof(bool)),
                new EntityField<ShareProductResponse>(dto => dto.Status, "Status", "Status"),
            ],
            searchFunc: async filter =>
            {
                var request = new SearchShareProductsCommand
                {
                    PageNumber = filter.PageNumber,
                    PageSize = filter.PageSize,
                    Keyword = filter.Keyword,
                    OrderBy = filter.OrderBy
                };
                var result = await Client.SearchShareProductsAsync("1", request).ConfigureAwait(false);
                return result.Adapt<PaginationResponse<ShareProductResponse>>();
            },
            enableAdvancedSearch: true,
            idFunc: dto => dto.Id,
            createFunc: async viewModel =>
            {
                await Client.CreateShareProductAsync("1", viewModel.Adapt<CreateShareProductCommand>()).ConfigureAwait(false);
            },
            updateFunc: async (id, viewModel) =>
            {
                await Client.UpdateShareProductAsync("1", id, viewModel.Adapt<UpdateShareProductCommand>()).ConfigureAwait(false);
            },
            deleteFunc: async id => await Client.DeleteShareProductAsync("1", id).ConfigureAwait(false),
            entityName: "Share Product",
            entityNamePlural: "Share Products",
            entityResource: FshResources.ShareProducts);

        await AuthState;
    }

    private async Task ShowShareProductsHelp()
    {
        await DialogService.ShowAsync<ShareProductsHelpDialog>("Share Products Help", new DialogParameters(), new DialogOptions
        {
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            CloseOnEscapeKey = true
        });
    }

    private async Task ViewProductDetails(DefaultIdType id)
    {
        var product = await Client.GetShareProductAsync("1", id).ConfigureAwait(false);
        if (product != null)
        {
            var parameters = new DialogParameters { ["Product"] = product };
            await DialogService.ShowAsync<ShareProductDetailsDialog>("Share Product Details", parameters, new DialogOptions
            {
                MaxWidth = MaxWidth.Medium,
                FullWidth = true,
                CloseOnEscapeKey = true
            });
        }
    }

    private void ViewShareAccounts(DefaultIdType productId)
    {
        Navigation.NavigateTo($"/microfinance/share-accounts?productId={productId}");
    }
}
